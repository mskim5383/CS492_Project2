from django.shortcuts import render
from django.http import HttpResponse
from server.models import *
import json

# Create your views here.



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
    if not Player.objects.filter(id=user_id).exists():
        return HttpResponse(json.dumps({'status': 'invalid user'}), content_type="application/json")
    game_status = GameStatus()
    game_status.player1 = Player.objects.get(id=user_id)
    game_status.save()
    return HttpResponse(json.dumps(game_status.get_game_status()), content_type="application/json")

def game_room(request, user_id, room_id):
    return HttpResponse(json.dumps({}), content_type="application/json")

