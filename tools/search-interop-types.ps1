$interop = "C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver\BepInEx\interop"

$bytes = [System.IO.File]::ReadAllBytes("$interop\Assembly-CSharp.dll")
$text = [System.Text.Encoding]::ASCII.GetString($bytes)

Write-Host "=== Searching for exact missing type names ==="
$terms = @("SeahorseRace", "CardGame", "CardMini", "BangshaCard", "ChapterInfo", "ChapterManager", "FarmSave", "FishFarmArea", "ObscuredBool", "ArchipelagoSession", "ReceivedItems", "SaveSystem", "EnumBoss")
foreach ($term in $terms) {
    if ($text -match $term) { Write-Host "FOUND: $term" } else { Write-Host "NOT FOUND: $term" }
}

Write-Host ""
Write-Host "=== All type names containing 'Race' ==="
$matches = [regex]::Matches($text, '[A-Za-z][A-Za-z0-9]*Race[A-Za-z0-9]*')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "=== All type names containing 'Card' ==="
$matches = [regex]::Matches($text, '[A-Za-z][A-Za-z0-9]*Card[A-Za-z0-9]*')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "=== All type names containing 'Chapter' ==="
$matches = [regex]::Matches($text, '[A-Za-z][A-Za-z0-9]*Chapter[A-Za-z0-9]*')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "=== All type names containing 'Boss' ==="
$matches = [regex]::Matches($text, '[A-Za-z][A-Za-z0-9]*Boss[A-Za-z0-9]*')
$matches | ForEach-Object { $_.Value } | Sort-Object -Unique | ForEach-Object { Write-Host $_ }
