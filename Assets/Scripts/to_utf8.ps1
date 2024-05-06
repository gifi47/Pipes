# Specify the directory containing the files
$directory = $PSScriptRoot

# Get all files in the directory
$files = Get-ChildItem -Path $directory -Recurse -File | Where-Object { $_.Extension -ne '.meta' }

# Loop through each file
foreach ($file in $files) {
    # Read the file content with the correct encoding
    $content = Get-Content -Path $file.FullName -Raw

    # Write the content back to the file with UTF-8 encoding
    Set-Content -Path $file.FullName -Value $content -Encoding UTF8
}

Write-Output "Conversion complete."


