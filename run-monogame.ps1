# Kill the old site
rm .\_site\monogame\ -force -recurse

docfx metadata .\monogame\docfx.json

if ($args.Count -eq 1) {
  docfx build .\monogame\docfx.json --serve
} else {
  docfx build .\monogame\docfx.json
}