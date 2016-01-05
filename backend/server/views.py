from django.shortcuts import render
from django.http import HttpResponse
from django.views.decorators.csrf import csrf_exempt
from server.models import *
import json
import random

# Create your views here.

FACE = ['S', 'D', 'H', 'C']
VALUE = ['A', '2', '3', '4', '5', '6', '7', '8', '9', '10', 'J', 'Q', 'K']


def register(request):
    player = Player()
    player.save()
    return HttpResponse(json.dumps({'id': player.id}), content_type="application/json")

def waiting_room(request, user_id):
    if not Player.objects.filter(id=user_id).exists():
        return HttpResponse(json.dumps({'status': 'invalid user'}), content_type="application/json")
    game_status_list = GameStatus.objects.all()
    waiting_room_list = []
    for game_status in game_status_list:
        waiting_room_list.append(game_status.get_game_status())
    return HttpResponse(json.dumps({'status': 'stable', 'waiting_rooms': waiting_room_list}), content_type="application/json")

def create_room(request, user_id):
    if Player.objects.filter(id=user_id).exists():
        player = Player.objects.get(id=user_id)
    else:
        return HttpResponse(json.dumps({'status': 'invalid user'}), content_type="application/json")
    if not player.game_status is None:
        return HttpResponse(json.dumps({'status': 'already joined'}), content_type="application/json")
    game_status = GameStatus()
    game_status.save()
    player.game_status = game_status
    player.order = 0
    player.save()
    return HttpResponse(json.dumps(game_status.get_game_status()), content_type="application/json")

@csrf_exempt
def game_room(request, user_id, room_id):
    if Player.objects.filter(id=user_id).exists():
        player = Player.objects.get(id=user_id)
    else:
        return HttpResponse(json.dumps({'status': 'invalid user'}), content_type="application/json")
    if GameStatus.objects.filter(id=room_id).exists():
        game_status = GameStatus.objects.get(id=room_id)
    else:
        return HttpResponse(json.dumps({'status': 'invalid room id'}), content_type="application/json")
    status = game_status.status
    try:
        data = json.loads(request.body)
    except:
        data = {}
    player.save()
    try:
        if status == 0:
            status0(data, player, game_status)
        elif player.game_status == game_status and player.order == game_status.turn and data.has_key('action'):
            if status == 1:
                status1(data, player, game_status)
            elif status == 2:
                status2(data, player, game_status)
            elif status == 3:
                status3(data, player, game_status)
    except ValueError:
        pass



    return HttpResponse(json.dumps({'game_status':game_status.get_game_status(),
                                    'player':player.get_player()}), content_type="application/json")

def status0(data, player, game_status):
    if player.game_status == game_status:
        return
    order = game_status.add_player(player)
    if game_status.players.count() < 5:
        return
    game_status.status = 1
    game_status.turn = 0
    game_status.lead = 0
    game_status.save()
    cards = []
    for face in FACE:
        for value in VALUE:
            cards.append(face+value)
    cards.append('Jk')
    random.shuffle(cards)
    game_status.set_remain_cards(cards[:3])
    i = 0
    for player in game_status.players.all():
        player.set_hands(cards[3+10*i:13+10*i])
        i += 1


    return

def status1(data, player, game_status):
    if not player.passed:
        action = data.get('action')
        if action == 'contract':
            if not game_status.contract is None and int(data.get('number')) <= game_status.contract.number:
                return
            if int(data.get('number')) < game_status.contract_limit:
                return
            contract = Contract()
            contract.face = data.get('face')
            contract.number = int(data.get('number'))
            contract.player = player
            contract.save()
            game_status.contract = contract
            player_list = game_status.get_player_list()
            if game_status.players.filter(passed=False).count() == 1:
                game_status.status = 2
                game_status.lead = game_status.contract.player.order
                game_status.turn = game_status.contract.player.order
                game_status.declarer = game_status.contract.player.order
                p = game_status.contract.player
                p.set_hands(p.get_hands() + game_status.get_remain_cards())
            else:
                for p in player_list[(player.order + 1) % 5:] + player_list[:(player.order + 1) % 5]:
                    if not p.passed:
                        break
                game_status.turn = p.order
        elif action == 'pass':
            player.passed = True
            player.save()
            count = game_status.players.filter(passed=False).count()
            player_list = game_status.get_player_list()
            if count == 0:
                for p in player_list:
                    p.passed = False
                    p.save()
                game_status.contract_limit -= 1
                game_status.turn = 0
            elif count == 1 and not game_status.contract is None:
                game_status.status = 2
                game_status.lead = game_status.contract.player.order
                game_status.turn = game_status.contract.player.order
                game_status.declarer = game_status.contract.player.order
                p = game_status.contract.player
                p.set_hands(p.get_hands() + game_status.get_remain_cards())
            else:
                for p in player_list[(player.order + 1) % 5:] + player_list[:(player.order + 1) % 5]:
                    if not p.passed:
                        break
                game_status.turn = p.order

    game_status.save()
    return

def status2(data, player, game_status):
    action = data.get('action')
    if action == 'remain' and len(player.get_hands()) == 13:
        hands = player.get_hands()
        cards = data.get('cards')
        if len(cards) != 8:
            return
        cards = cards.split(',')
        for card in cards:
            if not card in hands:
                return
        for card in cards:
            hands.pop(hands.index(card))
        game_status.set_remain_cards(cards)
        player.set_hands(hands)
        player.save()
    elif action == 'friend' and not game_status.friend:
        game_status.friend = int(data.get('friend'))
        if game_status.friend == 2:
            game_status.friend_card_face = data.get('face')
            game_status.friend_card_value = data.get('value')
        elif game_status.friend == 3:
            game_status.friend_select = int(data.get('select'))
    if game_status.friend and len(player.get_hands()) == 10:
        game_status.status += 1
    game_status.save()
    return

def status3(data, player, game_status):
    action = data.get('action')
    hands = player.get_hands()
    giruda = game_status.contract.face
    card = data.get('card')
    mighty = 'SA'
    if giruda == 'S':
        mighty = 'CA'
    if game_status.joker_call and 'Jk' in hands and card != mighty:
        hands.pop(hands.index('Jk'))
        player.set_hands(hands)
        game_status.add_trick(player, 'Jc')
        lead_face = game_status.lead_face
    else:
        if game_status.turn == game_status.lead:
            if card == 'Jk':
                game_status.lead_face = data.get('face')
            else:
                game_status.lead_face == card[0]
        lead_face = game_status.lead_face
        if action == 'joker_call':
            if game_status.contract.face == 'C':
                joker_call = 'S3'
            else:
                joker_call = 'C3'
            if joker_call != card:
                return
            if not joker_call in hands:
                return
            hands.pop(hands.index(joker_call))
            player.set_hands(hands)
            game_status.add_trick(player, joker_call)
            game_status.joker_call = True
        elif action == 'throw':
            if not card in hands:
                return
            if not card == 'Jk' and card[0] != lead_face:
                for hand in hands:
                    if hand[0] == lead_face:
                        return
            hands.pop(hands.index(card))
            player.set_hands(hands)
            game_status.add_trick(player, card)
        else:
            return
    game_status.turn = (game_status.turn + 1) % 5
    if game_status.turn == game_status.lead:
        point_card = []
        boss_card = {'card': '_0'}
        trick = game_status.get_trick()
        for card in trick:
            if card['card'][1:] in ('10', 'J', 'Q', 'K', 'A'):
                point_card.append(card['card'])
            if get_card_score(card['card'], giruda, lead_face) > get_card_score(boss_card['card'], giruda, lead_face):
                boss_card = card
        lead = boss_card['order']
        game_status.lead = lead
        game_status.turn = lead
        game_status.clear_trick()
        game_status.get_player_list()[lead].add_point_cards(point_card)
        game_status.joker_call = False
        if len(hands) == 0:
            game_status.status += 1
    game_status.save()
    return

def get_card_score(card, giruda, lead_face):
    score = 0
    if card == 'Jk':
        return 1000
    elif giruda == 'S' and card == 'CA':
        return 10000
    elif card == 'SA':
        return 10000
    else:
        if card[0] == giruda:
            score += 200
        elif card[0] == lead_face:
            score += 100
        score += {'0': 0, '2': 2, '3': 3, '4': 4, '5': 5, '6': 6, '7': 7, '8': 8, '9': 9, '10': 10, 'J': 11, 'Q': 12, 'K': 13, 'A': 14}[card[1:]]
    return score



