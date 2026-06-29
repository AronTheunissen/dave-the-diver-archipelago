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
Write-Host "=== All type names containing 'ChapterInfo' ==="
$matches = [regex]::Matches($text, '[A-Za-z][A-Za-z0-9]*ChapterInfo[A-Za-z0-9]*')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "=== set_currentChapterInfo parameter type — search nearby strings ==="
$idx = $text.IndexOf("set_currentChapterInfo")
if ($idx -ge 0) { Write-Host $text.Substring([Math]::Max(0,$idx-100), 300) }

Write-Host ""
Write-Host "=== Standalone VIPCustomer class name ==="
$matches = [regex]::Matches($text, '\bVIPCustomer\b')
$matches | ForEach-Object { Write-Host "Found at offset $($_.Index): $($text.Substring([Math]::Max(0,$_.Index-50), 120))" } | Select-Object -First 5

Write-Host ""
Write-Host "=== ObscuredBool namespace ==="
$idx = $text.IndexOf("ObscuredBool")
if ($idx -ge 0) { Write-Host $text.Substring([Math]::Max(0,$idx-200), 400) }
