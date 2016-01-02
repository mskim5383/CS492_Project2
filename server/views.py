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
    game_status_list = GameStatus.objects.all()
    waiting_room_list = []
    for game_status in game_status_list:
        waiting_room_list.append(game_status.get_game_status())
    return HttpResponse(json.dumps({'waiting_rooms': waiting_room_list}), content_type="application/json")
