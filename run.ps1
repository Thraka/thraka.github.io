# Kill the old site
rm .\_site\ -force -recurse

if ($args.Count -eq 1) {

  Write-Host "==================="
  Write-Host "Rebuild API docs..."
  Write-Host "==================="

  docfx metadata .\root\docfx.json
}

Write-Host "================"
Write-Host "Building site..."
Write-Host "================"
docfx build .\root\docfx.json --serve
