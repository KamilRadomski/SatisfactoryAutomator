# SatisfactoryAutomator
Blazor Web Assembly SPA to make magic happen

## Recent Updates
- 2/5 Fixed hosting issue not seeing icons properly
      - Was not casing path correctly (IIS was fine but hosting in CloudFlare or Github Codespaces would not find icons)

## Known Issues
- Refresh not done on Home causes issue in Parser
- Clicking Ore recipes crashes - Converter in 'producedIn' building does not exist and page will be replaced with a extraction page later

## Current Tasks
- Build out Settings page and state container for settings
- Connect Settings to Codex to alter button appearance for default items

## UI Touch up Tasks
- Styling of defaulted recipes in Codex (slightly obnoxious looking currently)
