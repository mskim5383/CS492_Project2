from django.conf.urls import url
from django.contrib import admin

urlpatterns = [
    url(r'^$', 'server.views.waiting_room'),
]
