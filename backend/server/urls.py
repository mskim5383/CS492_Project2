from django.conf.urls import url
from django.contrib import admin

urlpatterns = [
    url(r'^$', 'server.views.register'),
    url(r'^([1-9][0-9]*)/$', 'server.views.waiting_room'),
    url(r'^([1-9][0-9]*)/create_room/$', 'server.views.create_room'),
    url(r'^([1-9][0-9]*)/([1-9][0-9]*)/$', 'server.views.game_room'),
]
