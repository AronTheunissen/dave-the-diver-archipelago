$interop = "C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver\BepInEx\interop"

$bytes = [System.IO.File]::ReadAllBytes("$interop\Assembly-CSharp.dll")
$text = [System.Text.Encoding]::ASCII.GetString($bytes)

Write-Host "=== Exact nested class names under SaveData ==="
$matches = [regex]::Matches($text, 'SaveData[+/][A-Za-z0-9]+')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "=== All type names containing 'FarmSave' ==="
$matches = [regex]::Matches($text, '[A-Za-z][A-Za-z0-9]*FarmSave[A-Za-z0-9]*')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "=== All type names containing 'FishFarm' ==="
$matches = [regex]::Matches($text, '[A-Za-z][A-Za-z0-9]*FishFarm[A-Za-z0-9]*')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "=== All type names containing 'Obscured' ==="
$matches = [regex]::Matches($text, '[A-Za-z][A-Za-z0-9]*Obscured[A-Za-z0-9]*')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "=== All type names containing 'Chapter' (short list) ==="
$matches = [regex]::Matches($text, '\b(ChapterInfo|ChapterManager|ChapterData|ChapterSave)\b')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "=== All type names containing 'VIPCustomer' ==="
$matches = [regex]::Matches($text, '[A-Za-z][A-Za-z0-9]*VIP[A-Za-z0-9]*')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }
