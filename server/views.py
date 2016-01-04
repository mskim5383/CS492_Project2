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
    player.order = 1
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
    if game_status.status == 0:
        status0(request, player, game_status)
    elif game_status.status == 1:
        status1(request, player, game_status)



    return HttpResponse(json.dumps(game_status.get_game_status()), content_type="application/json")

def status0(request, player, game_status):
    if player.game_status == game_status:
        return
    order = game_status.add_player()
    if game_status.players.count() < 4:
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
    game_status.set_cards(cards[3:])
    return

def status1(request, player, game_status):
    if game_status.turn == player.order and not player.passed and request.GET.has_key('action'):
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
                game_status.limit -= 1
                game_status.turn = 0
            elif count == 1:
                if not game_status.contract is None:
                    game_status.status = 2
                    game_status.lead = game_status.contract.player.order
                    game_status.turn = game_status.contract.player.order
                    game_status.declarer = game_status.contract.player.order
            else:
                for p in player_list[(player.order + 1) % 5:] + player_list[:(player.order + 1) % 5]:
                    if not p.passed:
                        break
                game_status.turn = p.order

    game_status.save()
    return


