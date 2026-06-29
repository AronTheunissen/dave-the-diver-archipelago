$interop = "C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver\BepInEx\interop"

$terms = @("FarmSave", "FishFarm", "Seahorse", "CardGame", "VIPCustomer", "VipCustomer", "Chapter", "SaveSystem", "ObscuredBool", "Obscured")

foreach ($term in $terms) {
    $results = Select-String -Path "$interop\Assembly-CSharp.dll" -Pattern $term -Encoding Byte -List
    if ($results) { Write-Host "FOUND '$term' in Assembly-CSharp.dll" }
}

foreach ($dll in Get-ChildItem "$interop\*.dll") {
    $results = Select-String -Path $dll.FullName -Pattern "ObscuredBool" -Encoding Byte -List
    if ($results) { Write-Host "FOUND 'ObscuredBool' in $($dll.Name)" }
}
