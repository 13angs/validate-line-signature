# Validate Line signature

## Dotnet core

- cd or enter into dotnetcore folder
- copy and rename the `appsetting.json` to `appsetting.Development.json`

- in `appsetting.Development.json` update the env as below

```json
{
  ...
  "LineProvider": {
    "ClientId": "<your_line_client_id>",
    "SecretId": "<your_line_secret_id>"
  }
}
```

- run the api

```bash
dotnet run
```

- expose to the world using ngrok
  - need to install ngrok in linux first
  - inside devcontainer add the token using

```bash
/home/vscode/ngrok/ngrok config add-authtoken <your_ngrok_token>
```

- run the ngrok command

```bash
/home/vscode/ngrok/ngrok http <api_port>
```

- copy the url and paste in Line console 

exp:

```bash
https://afd1-171-7-226-34.ap.ngrok.io/webhook/v1/line/1657615668
```

- try sending the message from your Line OA
