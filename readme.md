# Game Rules
- This is a game of chance in which a random number between 0 - 9 is to be generated.
- A player will predict the random number.
- The player has a starting account of 10,000 points and can wager them on a prediction which they will either win or lose.
- Any number of points can be wagered.
- If the player is correct, they win 9 times their stake.
- If the player is incorrect, they lose their stake.

## Task
- The player sends their bet as a request to the program.
  - Example:
    ```json
    {
      "points": 100,
      "number": 3
    }
    ```

- If the bet is successful, the player's account balance is returned.
  - Example:
    ```json
    {
      "account": 10900,
      "status": "won",
      "points": "+900"
    }
    ```

## Tips
- Error handling.
- Identification of players.
