# Test project #

A test project to learn .NET Core, and familiarize myself with the .NET ecosystem.

For now, the code itself is a simple web API that has endpoints for a creating new rooms, and making room reservations.

TO-DO:

* Implement simple authorization, with two levels of user access:
  * Manager: Can create new rooms, change existing reservations, and approve reservations
  * Guest: Can create a new reservation, manage their own reservations
  * Endpoints to create and update these users
