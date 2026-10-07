<#
.SYNOPSIS
    P0.E8.T1 - asset license ledger helper for docs/legal/asset-ledger.md.

.DESCRIPTION
    Scans Assets/Audio, Assets/Sprites and font files under Assets, reads each
    file's Unity GUID from its .meta, and parses the license token from audio
    filenames (project convention: <name>_<license>.wav / .mp3 - see
    docs/essential/assets.md "Naming conventions").

    Default : print markdown rows to paste into the ledger tables.
    -Check  : verify every scanned asset appears in the ledger (path + GUID)
              and that the ledger has no stale asset rows (asset removed from
              the project but still listed). Exit 0 = clean, 1 = drift, 2 = error.

.EXAMPLE
    powershell -NoProfile -File scripts/get-asset-ledger.ps1
    powershell -NoProfile -File scripts/get-asset-ledger.ps1 -Check
#>
[CmdletBinding()]
param(
    # Repo root = parent of the scripts folder.
    [string]$Root = (Split-Path -Parent $PSScriptRoot),
    [string]$Ledger = '',
    [switch]$Check
)

$ErrorActionPreference = 'Stop'
if (-not $Ledger) { $Ledger = Join-Path $Root 'docs/legal/asset-ledger.md' }

$scanFolders = @('Assets/Audio', 'Assets/Sprites')
# Extensions treated as "ledger assets" (audio, sprites, fonts). .meta excluded.
$assetExtensions = @('.wav', '.mp3', '.ogg', '.png', '.jpg', '.jpeg', '.psd',
                     '.ttf', '.otf', '.fontsettings')
$fontExtensions = @('.ttf', '.otf', '.fontsettings')

function Get-UnityGuid([string]$file) {
    $meta = "$file.meta"
    if (-not (Test-Path -LiteralPath $meta)) { return '' }
    $hit = Select-String -Path $meta -Pattern 'guid:\s*([0-9a-f]{32})' | Select-Object -First 1
    if ($null -eq $hit) { return '' }
    return $hit.Matches[0].Groups[1].Value
}

function Get-LicenseToken([string]$name) {
    if ($name -match 'CC_BY_NC_SA') { return 'CC BY-NC-SA 3.0' }
    if ($name -match 'CCBY0')       { return 'CC BY 0.0' }
    if ($name -match 'CC0')         { return 'CC0 1.0' }
    if ($name -match 'CCBY3')       { return 'CC BY 3.0' }
    return ''
}

$rootFull = (Resolve-Path -LiteralPath $Root).Path
$assets = New-Object System.Collections.Generic.List[object]

function Add-Asset([System.IO.FileInfo]$file) {
    if ($file.Extension -notin $script:assetExtensions) { return }
    $relPath = $file.FullName.Substring($script:rootFull.Length + 1) -replace '\\', '/'
    $script:assets.Add([pscustomobject]@{
        Path    = $relPath
        Guid    = Get-UnityGuid $file.FullName
        License = Get-LicenseToken $file.Name
    })
}

foreach ($rel in $scanFolders) {
    $dir = Join-Path $rootFull ($rel -replace '/', '\')
    if (-not (Test-Path -LiteralPath $dir)) { continue }
    Get-ChildItem -LiteralPath $dir -Recurse -File | ForEach-Object { Add-Asset $_ }
}
# Fonts live anywhere under Assets (none today - the scan proves it stays true).
Get-ChildItem -LiteralPath (Join-Path $rootFull 'Assets') -Recurse -File |
    Where-Object { $_.Extension -in $fontExtensions } | ForEach-Object { Add-Asset $_ }

$assets = @($assets | Sort-Object Path)
if ($assets.Count -eq 0) {
    Write-Error 'No assets found - check -Root (expected the repo root).'
    exit 2
}

if (-not $Check) {
    foreach ($a in $assets) {
        Write-Output ('| `{0}` | `{1}` | {2} |' -f $a.Path, $a.Guid, $a.License)
    }
    exit 0
}

# ---- -Check: ledger must list every asset, and nothing that no longer exists.
if (-not (Test-Path -LiteralPath $Ledger)) {
    Write-Error "Ledger not found: $Ledger"
    exit 2
}
$ledgerText = Get-Content -LiteralPath $Ledger -Raw

$missingPath = @()
$missingGuid = @()
foreach ($a in $assets) {
    if (-not $ledgerText.Contains('`' + $a.Path + '`')) { $missingPath += $a.Path }
    if ($a.Guid -and -not $ledgerText.Contains($a.Guid)) { $missingGuid += $a.Path }
}

# Stale = ledger mentions an asset file (by extension) that is not in the project anymore.
$listed = [regex]::Matches($ledgerText, '`(Assets/[^`]+)`') |
    ForEach-Object { $_.Groups[1].Value } |
    Where-Object { [System.IO.Path]::GetExtension($_) -in $assetExtensions } |
    Sort-Object -Unique
$current = @($assets | ForEach-Object { $_.Path })
$stale = @($listed | Where-Object { $_ -notin $current })

$drift = $missingPath.Count + $missingGuid.Count + $stale.Count
if ($drift -gt 0) {
    foreach ($p in $missingPath) { Write-Output "::error::Asset not in ledger: $p" }
    foreach ($p in $missingGuid) { Write-Output "::error::Ledger GUID mismatch/missing: $p" }
    foreach ($p in $stale)       { Write-Output "::error::Ledger lists removed asset: $p" }
    Write-Output "Ledger drift: $drift issue(s) - update docs/legal/asset-ledger.md (P0.E8.T1)."
    exit 1
}
Write-Output ("Ledger OK - {0} assets listed with matching GUIDs." -f $assets.Count)
exit 0
