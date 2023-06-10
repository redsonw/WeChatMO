$ErrorActionPreference = "Stop"

# 获取当前脚本所在的目录
$currentDir = Split-Path -Parent $MyInvocation.MyCommand.Path

# 读取 csproj 文件
$csprojFile = Join-Path -Path $currentDir -ChildPath "WeChatMultiOpen.csproj"
$csproj = [xml](Get-Content -Path $csprojFile)

Write-Host "${csproj.Version}"

# 获取所有包含 Version 属性的 PropertyGroup 元素
$propertyGroups = $csproj.Project.PropertyGroup | Where-Object { $_.Version }

# 输出版本号
foreach ($propertyGroup in $propertyGroups) {
    $version = $propertyGroup.Version
    $ssemblyVersion = $propertyGroup.AssemblyVersion
    $FileVersion = $propertyGroup.FileVersion

    Write-Host "版本: $version"
    Write-Host "程序版本: $ssemblyVersion"
    Write-Host "文件版本: $FileVersion"
}

# 获取当前版本号的各个部分
$major = [int]$propertyGroups.MajorVersion
$minor = [int]$propertyGroups.MinorVersion
$patch = [int]$propertyGroups.PatchVersion
$build = [int]$propertyGroups.BuildVersion

Write-Host "当前版本：$major.$minor.$patch.$build"

# 自增 Build 版本号并处理满9999的情况
if ($build -lt 9999) {
    $build++
} else {
    $build = 0
    if ($patch -lt 999) {
        $patch++
    } else {
        $patch = 0
        if ($minor -lt 99) {
            $minor++
        } else {
            $minor = 0
            $major++
        }
    }
}

Write-Host "合并版本: $major.$minor.$patch.$build"

# 更新版本号
$newVersion = "$major.$minor.$patch.$build"

foreach ($propertyGroup in $propertyGroups) {
    $propertyGroup.MajorVersion = $major.ToString()
    $propertyGroup.MinorVersion = $minor.ToString()
    $propertyGroup.PatchVersion = $patch.ToString()
    $propertyGroup.BuildVersion = $build.ToString()
    $propertyGroup.Version = $newVersion
    $propertyGroup.AssemblyVersion = $newVersion
    $propertyGroup.FileVersion = $newVersion
}

# 保存修改后的 csproj 文件
$csproj.Save($csprojFile)

# 输出更新后的版本号
Write-Host "版本号已更新为: $newVersion"
