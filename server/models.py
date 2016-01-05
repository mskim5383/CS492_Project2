# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models
import json

# Create your models here.



class GameStatus(models.Model):
    status = models.IntegerField(default=0)
    turn = models.IntegerField(default=0)
    lead = models.IntegerField(default=0)
    contract = models.ForeignKey('Contract', related_name='gamestatus', null=True, blank=True)
    contract_limit = models.IntegerField(default=15)
    remain_cards = models.CharField(max_length=200, default=json.dumps({'cards':[]}))
    trick = models.CharField(max_length=200, default=json.dumps({'trick':[]}))
    declarer = models.IntegerField(default=0)
    friend = models.IntegerField(default=0) # 1: 초구, 2: 카드, 3: 지정
    friend_card_face = models.CharField(max_length=2, blank=True)
    friend_card_value = models.CharField(max_length=2, blank=True)
    friend_select = models.IntegerField(default=0)


    def set_remain_cards(self, card):
        self.remain_cards = json.dumps({'cards': card})
        self.save()

    def get_remain_cards(self):
        return json.loads(self.remain_cards)['cards']

    def clear_trick(self):
        self.trick = json.dumps({'trick': []})

    def add_trick(self, player, card):
        trick = json.loads(self.trick)
        trick['trick'].append({'player': player, 'card': card})
        self.trick = json.dumps(trick)

    def get_trick(self):
        return json.loads(self.trick)['trick']

    def get_player(self, player):
        if player in self.players.all():
            return player.order
        return -1

    def get_player_list(self):
        player_list = [None] * 5
        for player in self.players.all():
            player_list[player.order] = player
        return player_list

    def add_player(self, player):
        if self.players.count() < 5:
            player.game_status = self
            player.order = self.players.count()
            player.save()
            return player.order
        return -1

    def get_game_status(self):
        status = {}
        status['id'] = self.id
        status['status'] = self.status
        status['turn'] = self.turn
        status['lead'] = self.lead
        if not self.contract is None:
            status['contract'] = self.contract.get_contract()
        else:
            status['contract'] = None
        status['contract_limit'] = self.contract_limit
        players = {}
        players['player0'] = {'id': None, 'passed': False, 'point_card': []}
        players['player1'] = {'id': None, 'passed': False, 'point_card': []}
        players['player2'] = {'id': None, 'passed': False, 'point_card': []}
        players['player3'] = {'id': None, 'passed': False, 'point_card': []}
        players['player4'] = {'id': None, 'passed': False, 'point_card': []}
        for player in self.players.all():
            players['player' + str(player.order)]['id'] = player.id
            players['player' + str(player.order)]['passed'] = player.passed
            players['player' + str(player.order)]['point_card'] = player.get_point_card()
        status['players'] = players
        status['remain_cards'] = self.get_remain_cards()
        status['trick'] = self.get_trick()
        status['declarer'] = self.declarer
        status['friend'] = self.friend
        status['friend_card_face'] = self.friend_card_face
        status['friend_card_value'] = self.friend_card_value
        status['friend_select'] = self.friend_select
        return status









class Contract(models.Model):
    face = models.CharField(max_length=2)
    number = models.IntegerField()
    player = models.ForeignKey('Player', related_name='contract', null=False)

    def get_contract(self):
        return {'face': self.face, 'number': self.number, 'player': self.player.id}


class Player(models.Model):
    hands = models.CharField(max_length=200, default=json.dumps({'hands':[]}))
    passed = models.BooleanField(default=False)
    point_card = models.CharField(max_length=200, default=json.dumps({'cards':[]}))
    game_status = models.ForeignKey('GameStatus', related_name='players', null=True)
    order = models.IntegerField(default=-1)


    def __unicode__(self):
        return u'Player %d. passed: %s game_status: %s order: %d' %(self.id, self.passed, self.game_status and self.game_status.id, self.order)

    def set_hands(self, hands):
        self.hands = json.dumps({'hands':hands})
        self.save()

    def get_hands(self):
        return json.loads(self.hands)['hands']

    def set_point_card(self, cards):
        self.point_card = json.dumps({'cards':cards})
        self.save()

    def get_point_card(self):
        return json.loads(self.point_card)['cards']

    def get_player(self):
        return {'hands': self.get_hands(), 'order': self.order, 'point_card': self.get_point_card()}

