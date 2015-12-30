from django.shortcuts import render
from django.http import HttpResponse
import json

# Create your views here.




def waiting_room(request):
    return HttpResponse(json.dumps({}), content_type="application/json")
