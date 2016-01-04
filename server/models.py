# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models
import json

# Create your models here.



class GameStatus(models.Model):
    status = models.IntegerField(default=0)
    turn = models.IntegerField(default=0)
    lead = models.IntegerField(default=0)
    contract = models.ForeignKey('Contract', related_name='gamestatus', null=True)
    remain_card = models.CharField(max_length=200, default=json.dumps({}))
    trick = models.CharField(max_length=200, default=json.dumps({}))
    declarer = models.IntegerField(default=0)
    friend = models.IntegerField(default=0) # 1: 초구, 2: 카드, 3: 지정
    friend_card_face = models.IntegerField(default=-1)
    friend_card_value = models.IntegerField(default=-1)
    friend_select = models.IntegerField(default=0)

    def set_remain_card(self, card):
        self.remain_card = json.dumps({'cards': card})
        self.save()

    def get_remain_card(self):
        return json.loads(self.remain_card)

    def clear_trick(self):
        self.trick = json.dumps({'trick': []})

    def add_trick(self, player, card):
        trick = json.loads(self.trick)
        trick['trick'].append({'player': player, 'card': card})
        self.trick = json.dumps(trick)

    def get_trick(self):
        return json.loads(self.trick)

    def get_player(self, player):
        if player in self.players.all():
            return player.order
        return -1

    def add_player(self, player):
        if self.players.count() < 5:
            player.game_status = self
            player.order = self.players.count() + 1
            player.save()
            return player.order
        return -1

    def get_game_status(self):
        status = {}
        status['status'] = self.status
        status['turn'] = self.turn
        status['lead'] = self.lead
        if not self.contract is None:
            status['contract'] = self.contract.get_contract()
        else:
            status['contract'] = None
        status['player1'] = None
        status['player2'] = None
        status['player3'] = None
        status['player4'] = None
        status['player5'] = None
        for player in self.players.all():
            status['player' + str(player.order)] = player.id
        status['remain_card'] = self.get_remain_card()
        status['trick'] = self.get_trick()
        status['declarer'] = self.declarer
        status['friend'] = self.friend
        status['friend_card_face'] = self.friend_card_face
        status['friend_card_value'] = self.friend_card_value
        status['friend_select'] = self.friend_select
        return status









class Contract(models.Model):
    face = models.IntegerField()
    number = models.IntegerField()
    player = models.ForeignKey('Player', related_name='contract', null=False)

    def get_contract(self):
        return {'face': self.face, 'number': self.number, 'player': self.player.id}


class Player(models.Model):
    hands = models.CharField(max_length=200, default=json.dumps({}))
    passed = models.BooleanField(default=False)
    point_card = models.CharField(max_length=200)
    game_status = models.ForeignKey('GameStatus', related_name='players', null=True)
    order = models.IntegerField(default=0)

    def set_hands(self, hands):
        self.hands = json.dumps(hands)
        self.save()
    def get_hands(self):
        return json.loads(self.hands)
