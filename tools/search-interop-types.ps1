$interop = "C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver\BepInEx\interop"

$terms = @("FarmSave", "FishFarm", "Seahorse", "CardGame", "VIPCustomer", "VipCustomer", "Chapter", "SaveSystem", "ObscuredBool", "Obscured")

foreach ($term in $terms) {
    $bytes = [System.IO.File]::ReadAllBytes("$interop\Assembly-CSharp.dll")
    $text = [System.Text.Encoding]::ASCII.GetString($bytes)
    if ($text -match $term) { Write-Host "FOUND '$term' in Assembly-CSharp.dll" }
}

Write-Host ""
Write-Host "Searching all DLLs for ObscuredBool..."
foreach ($dll in Get-ChildItem "$interop\*.dll") {
    $bytes = [System.IO.File]::ReadAllBytes($dll.FullName)
    $text = [System.Text.Encoding]::ASCII.GetString($bytes)
    if ($text -match "ObscuredBool") { Write-Host "FOUND 'ObscuredBool' in $($dll.Name)" }
}
