# Mini Redis - Custom In-Memory Key-Value Store

A highly performant, raw C# implementation of an In-Memory Key-Value database. This project is built completely from scratch without relying on any external frameworks, focusing heavily on core backend architecture, memory management, and data persistence concepts.

## 🚀 Features

- **In-Memory Storage:** Uses C#'s `Dictionary<string, string>` for blazing fast, $O(1)$ time complexity data retrieval.
- **Data Persistence (WAL/AOF):** Implements Write-Ahead Logging (Append-Only File) to ensure no data is lost when the server shuts down.
- **Custom REPL Engine:** A custom command-line interface that listens to, parses, and executes database commands in real-time.
- **Auto-Recovery:** Automatically reconstructs the database state from the `.aof` log file into RAM upon server startup.

## 🛠️ Tech Stack

- **Language:** C# (.NET Core)
- **Architecture:** Raw Console Application (No Frameworks)
- **Storage Mechanism:** RAM (Dictionary) + Disk (System.IO StreamReader/Writer)

## 💻 Supported Commands

| Command | Usage               | Description                                                                                           |
| :------ | :------------------ | :---------------------------------------------------------------------------------------------------- |
| `SET`   | `SET [key] [value]` | Stores or updates the value associated with the key in RAM and appends the operation to the log file. |
| `GET`   | `GET [key]`         | Retrieves the value of the specified key from RAM. Returns `(nil)` if the key does not exist.         |
| `DEL`   | `DEL [key]`         | Removes the key-value pair from RAM and logs the deletion to keep persistence in sync.                |
| `EXIT`  | `EXIT`              | Safely shuts down the Mini Redis server.                                                              |

## 🧠 How it Works (Under the Hood)

1.  **Operation:** When a user executes a `SET` or `DEL` command, the engine first applies the mutation to the in-memory dictionary for instant availability.
2.  **Persistence:** Immediately after, the exact command string is appended to the `appendonly.aof` file on the disk.
3.  **Recovery:** On the next boot, the engine reads `appendonly.aof` line-by-line and "replays" the commands in the background to reconstruct the exact state of the dictionary before accepting new client connections.

## 🏃‍♂️ How to Run

1. Clone the repository.
2. Open the project directory in your terminal.
3. Run the following command:
   ```bash
   dotnet run
   ```
4. Start typing commands in the 127.0.0.1:6379> prompt!
