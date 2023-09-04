#docfx .\v8\docfx.json build
#docfx .\v9\docfx.json build
rm .\_site\ -force -recurse
docfx build .\root\docfx.json --serve
