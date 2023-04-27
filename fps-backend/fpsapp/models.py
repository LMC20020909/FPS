from django.db import models


class Room(models.Model):
    name = models.CharField(default="Room", max_length=100)
    port = models.IntegerField(default=7777)

    def _str_(self):
        return self.name + ' - ' + str(self.port)
