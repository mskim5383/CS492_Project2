# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models
import json

# Create your models here.



class GameStatus(models.Model):
    status = models.IntegerField(default=0)
    turn = models.IntegerField(default=0)
    lead = models.IntegerField()
    contract = models.ForeignKey('Contract', related_name='gamestatus')
    player1 = models.ForeignKey('Player', related_name='gamestatus1')
    player2 = models.ForeignKey('Player', related_name='gamestatus2')
    player3 = models.ForeignKey('Player', related_name='gamestatus3')
    player4 = models.ForeignKey('Player', related_name='gamestatus4')
    player5 = models.ForeignKey('Player', related_name='gamestatus5')
    remain_card = models.CharField(max_length=200)
    trick = models.CharField(max_length=200)
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
        player_list = [self.player1, self.player2, self.player3, self.player4, self.player4]
        for i in range(5):
            if (not player_list[i] is None) and player_list[i] == player:
                return i + 1
        return -1

    def add_player(self, player):
        if self.player1 is None:
            self.player1 = player
            self.save()
            return 1
        if self.player2 is None:
            self.player2 = player
            self.save()
            return 2
        if self.player3 is None:
            self.player3 = player
            self.save()
            return 3
        if self.player4 is None:
            self.player4 = player
            self.save()
            return 4
        if self.player5 is None:
            self.player5 = player
            self.save()
            return 5
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
        if not self.player1 is None:
            status['player1'] = self.player1.id
        else:
            status['player1'] = None
        if not self.player2 is None:
            status['player2'] = self.player2.id
        else:
            status['player2'] = None
        if not self.player3 is None:
            status['player3'] = self.player3.id
        else:
            status['player3'] = None
        if not self.player4 is None:
            status['player4'] = self.player4.id
        else:
            status['player4'] = None
        if not self.player5 is None:
            status['player5'] = self.player5.id
        else:
            status['player5'] = None
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
    hands = models.CharField(max_length=200)
    passed = models.BooleanField(default=False)
    point_card = models.CharField(max_length=200)

    def set_hands(self, hands):
        self.hands = json.dumps(hands)
        self.save()
    def get_hands(self):
        return json.loads(self.hands)
