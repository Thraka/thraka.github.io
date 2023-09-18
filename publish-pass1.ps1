# Kill the old site
rm .\_site\ -force -recurse

Write-Host "===================="
Write-Host "Rebuild Monogame API"
Write-Host "===================="
docfx metadata .\monogame\docfx.json
docfx build .\monogame\docfx.json

Write-Host "================"
Write-Host "Building site..."
Write-Host "================"
docfx metadata .\root\docfx.json
docfx build .\root\docfx.json
