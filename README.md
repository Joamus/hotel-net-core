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

* (Rudimentary) unit tests

### To-do ###

* Clean up error handling (done, mostly)

* Make "room-purchase" handling
  * A hotel guest can put purchases on their room, which they should pay for at check-out
  * A manager can give discount to specific purchases
  * A reservation can have several guests
