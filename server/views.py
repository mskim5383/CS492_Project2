from django.shortcuts import render
from django.http import HttpResponse
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
    game_status = GameStatus()
    game_status.save()
    player.game_status = game_status
    player.order = 0
    player.save()
    return HttpResponse(json.dumps(game_status.get_game_status()), content_type="application/json")

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
    if status == 0:
        status0(request, player, game_status)
    elif player.game_status == game_status and player.order == game_status.turn and request.GET.has_key('action'):
        if status == 1:
            status1(request, player, game_status)
        elif status == 2:
            status2(request, player, game_status)



    return HttpResponse(json.dumps({'game_status':game_status.get_game_status(),
                                    'palyer':player.get_player()}), content_type="application/json")

def status0(request, player, game_status):
    if player.game_status == game_status:
        return
    order = game_status.add_player(player)
    if game_status.players.count() < 5:
        return
    game_status.status = 1
    game_status.turn = 0
    game_status.lead = 0
    cards = []
    for face in FACE:
        for value in VALUE:
            cards.append(face+value)
    cards.append('JK')
    random.shuffle(cards)
    game_status.set_remain_cards(cards[:3])
    player_list = game_status.get_player_list()
    for i in range(5):
        player_list[i].set_hands(cards[3+10*i:13+10*i])
        player_list[i].save()


    return

def status1(request, player, game_status):
    if not player.passed:
        action = request.GET.get('action')
        if action == 'contract':
            if not game_status.contract is None and int(request.GET.get('number')) <= game_status.contract.number:
                return
            if int(request.GET.get('number')) < game_status.contract_limit:
                return
            contract = Contract()
            contract.face = request.GET.get('face')
            contract.number = int(request.GET.get('number'))
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

def status2(request, player, game_status):
    action = request.GET.get('action')
    if action == 'remain' and len(player.get_hands()) == 13:
        hands = player.get_hands()
        cards = request.GET.get('cards')
        print cards
        if len(cards) != 8:
            return
        cards = cards.split(',')
        for card in cards:
            print card
            if not card in hands:
                return
        for card in cards:
            hands.pop(hands.index(card))
        game_status.set_remain_cards(cards)
        player.set_hands(hands)
        player.save()
    elif action == 'friend' and not game_status.friend:
        game_status.friend = int(request.GET.get('friend'))
        if game_status.friend == 2:
            game_status.friend_card_face = request.GET.get('face')
            game_status.friend_card_value = request.GET.get('value')
        elif game_status.friend == 3:
            game_status.friend_select = request.GET.get('select')
    if game_status.friend and len(player.get_hands()) == 10:
        game_status.status += 1
    game_status.save()
    return

