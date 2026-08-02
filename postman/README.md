# Postman Testing

This folder has everything you need to test the API in Postman — 15 requests
covering every endpoint plus the error cases (missing title, blank title,
unknown id, delete-twice).

## Files
- `Task-Manager-API.postman_collection.json` — the collection (import this)
\- `AspNet-Local.postman_environment.json` — points `baseUrl` at `http://localhost:5048`

## Setup
1. Start whichever API you want to test (see that project's README for run instructions).
2. In Postman: **Import** → drag in `Task-Manager-API.postman_collection.json` and  environment file.
3. In the top-right environment dropdown, pick **ASP.NET Core - Local** .
4. Click **Run** on the collection (or run requests individually, top to bottom — later requests depend on ids saved by requests 1 and 4).


## What's covered
| # | Request | Expects |
|---|---------|---------|
| 1 | Create task (valid) | 201 |
| 2 | Create task (missing title) | 400/422 |
| 3 | Create task (blank title) | 400/422 |
| 4 | Create task 2 (valid) | 201 |
| 5 | Get all tasks | 200, array |
| 6 | Get task by id | 200 |
| 7 | Get task by unknown id | 404 |
| 8 | PATCH status only | 200, other fields unchanged |
| 9 | PATCH blank title | 400/422 |
| 10 | PUT full update | 200 |
| 11 | PUT missing title | 400/422 |
| 12 | Filter by status=completed | 200, only matching tasks |
| 13 | Delete task | 204 |
| 14 | Get deleted task | 404 |
| 15 | Delete again | 404 |


```
