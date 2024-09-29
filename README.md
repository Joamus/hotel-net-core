# Test project #

A test project to learn .NET Core, and familiarize myself with the .NET ecosystem.

For now, the code itself is a simple web API that has endpoints for a creating new rooms, and making room reservations.

## Basic features ##

* Authorization with roles and policies
  * Guest role - a guest needs to sign up, before they can make reservations etc.

* Room endpoints
  * List all rooms (guest+ only)
  * Get a specific room (guest+ only)
  * Insert a new room (manager+ only)
  * Update a given room (manager+ only)
  * Delete room (could be used if a room is no longer to be used) (manager+ only)

* Room reservation endpoints
  * List all reservations - Guest-user will only be able to see their own reservations, while a manager can see all reservations
  * Get a single reservation - Guest can see own reservation, manager can see any
  * Insert a new reservation - Min. role is guest
  * Update a reservation - Guest can update own reservation, manager can update any.
  * Delete a reservation - Guest can delete own reservation, manager can delete any
  * Approve a reservation - Manager only

* (Rudementary) unit tests

### Notes ###

I experimented with moving the logic from the RoomReservationController to a RoomReservationController, so the controller itself is more bare-bones. I think this has a trade-off, since it introduces more complexity,
but the service also cannot return HTTP responses (since they are inherited from ControllerBase), which leaves these options:

a) Throw custom exceptions, that can be mapped to an error response
b) Return a result object, that can be mapped to an error response
c) Pass the controller as a parameter into the service method, so it can access the HTTP-response functions (practical but not very clean)

I chose to not do it for the RoomController, because the logic is fairly simple for now.

### To-do ###

* Clean up error handling

* Make "room-purchase" handling
  * A hotel guest can put purchases on their room, which they should pay for at check-out
  * A manager can give discount to specific purchases
  * A reservation can have several guests
