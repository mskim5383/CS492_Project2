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
        if player.game_status == game_status:
            return HttpResponse(json.dumps(game_status.get_game_status()), content_type="application/json")
        player.game_status = game_status
        player.order = game_status.players.count()
        player.save()
        if game_status.players.count() < 5:
            return HttpResponse(json.dumps(game_status.get_game_status()), content_type="application/json")
        game_status.status = 1
        game_status.turn = 1
        game_status.lead = 1
        cards = []
        for face in FACE:
            for value in VALUE:
                cards.append(face+value)
        cards.append('JK')
        random.shuffle(cards)
        game_status.set_remain_cards(cards[:3])
        game_status.set_cards(cards[3:])
        return HttpResponse(json.dumps(game_status.get_game_status()), content_type="application/json")



    return HttpResponse(json.dumps({}), content_type="application/json")

