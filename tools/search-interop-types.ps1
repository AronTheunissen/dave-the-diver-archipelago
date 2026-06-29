$interop = "C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver\BepInEx\interop"
$lib = "C:\Documenten\Code\dave-the-diver-archipelago\client\lib\interop"

Write-Host "=== Searching SaveSystem.dll for SaveSystem class ==="
$bytes2 = [System.IO.File]::ReadAllBytes("$lib\SaveSystem.dll")
$text2 = [System.Text.Encoding]::ASCII.GetString($bytes2)
$idx = $text2.IndexOf("SaveSystem")
if ($idx -ge 0) { Write-Host $text2.Substring([math]::Max(0,$idx-50), 200) }

Write-Host ""
Write-Host "=== Searching EzCoding.dll for ObscuredBool ==="
$bytes3 = [System.IO.File]::ReadAllBytes("$lib\EzCoding.dll")
$text3 = [System.Text.Encoding]::ASCII.GetString($bytes3)
$idx = $text3.IndexOf("ObscuredBool")
if ($idx -ge 0) { Write-Host $text3.Substring([math]::Max(0,$idx-100), 300) } else { Write-Host "NOT FOUND in EzCoding.dll" }

Write-Host ""
$bytes = [System.IO.File]::ReadAllBytes("$lib\Assembly-CSharp.dll")
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
Write-Host "=== set_currentChapterInfo context ==="
$idx = $text.IndexOf("set_currentChapterInfo")
if ($idx -ge 0) { Write-Host $text.Substring([math]::Max(0,$idx-100), 300) }

Write-Host ""
Write-Host "=== VIPCustomer context ==="
$idx = $text.IndexOf("VIPCustomer")
if ($idx -ge 0) { Write-Host $text.Substring([math]::Max(0,$idx-100), 300) }

Write-Host ""
Write-Host "=== ObscuredBool context ==="
$idx = $text.IndexOf("ObscuredBool")
if ($idx -ge 0) { Write-Host $text.Substring([math]::Max(0,$idx-100), 300) }

Write-Host ""
Write-Host "=== EnumBossFishType context ==="
$idx = $text.IndexOf("EnumBossFishType")
if ($idx -ge 0) { Write-Host $text.Substring([math]::Max(0,$idx-100), 300) } else { Write-Host "NOT FOUND" }

Write-Host ""
Write-Host "=== ChapterInfo context ==="
$idx = $text.IndexOf("ChapterInfo")
if ($idx -ge 0) { Write-Host $text.Substring([math]::Max(0,$idx-100), 300) } else { Write-Host "NOT FOUND" }

Write-Host ""
Write-Host "=== VIPCustomer class context ==="
$idx = $text.IndexOf("VIPCustomer")
if ($idx -ge 0) { Write-Host $text.Substring([math]::Max(0,$idx-100), 300) } else { Write-Host "NOT FOUND" }
