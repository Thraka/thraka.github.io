Write-Host "===================="
Write-Host "Rebuild Monogame API"
Write-Host "===================="
docfx build .\monogame\docfx.json

Write-Host "================"
Write-Host "Building site..."
Write-Host "================"
docfx build .\root\docfx.json
