# CQRS learning example
This repository contains an example CQRS app built with Xer.CQRS. This was an experiment to think through
how to design a CQRS application based on a given problem.

## Useful Links:
http://cqrs.nu/Faq

------------------
## Q and A:

Q:
Does the order events arrive in matter? I don't think it should, all events have a timestamp of when they occurred. Sagas and the like should probably
respect this when looking at events

A: Order of events should not matter. Sagas should be capable regardless of order events arrive to give eventual consistency.

Q: Where should random chance happen in the aggregate being this is a game?

A: Anything that is based on chance should occur in the application of commands. By the 
time events come through you want to know that applying an event will work the same every
time it is applied given the events that come before it are the same as well.

------------------
## Game Rules
damage modifiers:

Planes:
--vs soldiers
no damage
--vs tanks
stuka/sbd do 5 damage to a tank per hit with 80% chance to hit
mustang/bf109 do 3 damage to a tank per hit with 70% chance to hit
--vs planes
mustang/bf109 deal 100% damage against planes with a 70% chance to hit
stuka/sbd deal 100% damage against planes with a 50% chance to hit

Tanks:
--vs soldiers
Sherman/Panzer Tanks deal 100% damage against soldiers with a 70% chance to hit
Wolverine/Tiger do 100% damage against soldiers with a 50% chance to hit
--vs tanks
Sherman/Panzer do 3 damage to opposing tanks with 80% chance to hit
Wolverine/Tiger do 5 damage to opposing tanks with 80% chance to hit
--vs planes
no damage

Soldiers:
--vs soldiers
Soldiers do 100% dmg against opposing soldiers with 70% chance to hit
--vs tanks
Soldiers do 2 dmg against opposing tanks with 100% chance to hit
--vs planes
no damage

---------------------
limits:
--unit count
A team can have no more than:
5 tanks
5 planes
20 soldiers

--deployments per turn
each turn you get 5 deployment credits, infrantry take 1 credit, tanks take 3, and planes take 2. meaning you can do:
5 infantry in a turn, 3 infantry and a plane, 1 tank and 2 infantry etc.

---------------------
start
each team starts with 1 tiger/wolverine, 1 panzer/sherman, 1 bf109/mustang, 1 stuka/sbd, and 10 soldiers.
At the start of each round both players submit their deployment action as well as their target preference for each unit type.

Unit types (and health) are:
(8)Panzer/Sherman
(10)Wolverine/Tiger
(5)bf109/mustang
(7)stuka/sbd
(1)soldier





