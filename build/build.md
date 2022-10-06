# Structure of the release build

[pipeline-releasebuild.yaml](pipeline-releasebuild.yaml):
- [template-prebuild-code-analysis.yaml](template-prebuild-code-analysis.yaml)
  - 'Run PoliCheck'
  - 'Run CredScan'
  - 'Post Analysis'
- [template-bootstrap-build.yaml](template-bootstrap-build.yaml)
  - [template-install-dotnet-core.yaml](template-install-dotnet-core.yaml)
    - 'Use .Net Core SDK 5.0.100'
  - [template-install-nuget.yaml](template-install-nuget.yaml)

- [template-restore-build-MSIdentityAbstractions.yaml](template-restore-build-MSIdentityAbstractions.yaml) `(BuildPlatform:'$(BuildPlatform)', BuildConfiguration: '$(BuildConfiguration)', MSIdentityAbstractionsSemVer: $(MSIdentityAbstractionsSemVer))`
  - Build solution Microsoft.Identity.Abstractions.sln and run tests' (.NET Core)
  - [Build](template-restore-build-MSIdentityAbstractions.yaml) solution Microsoft.Identity.Abstractions.sln netcoreapp3.1 for Roslyn analyzers' (VSBuild@1)
  - 'Component Detection'
- [template-postbuild-code-analysis.yaml](template-postbuild-code-analysis.yaml)
  - 'Run Roslyn Analyzers'
  - 'Check Roslyn Results '
- [template-sign-binary.yaml](template-sign-binary.yaml) - Sign the binaries, requires dotnet core 2.x.
- [template-pack-and-sign-all-nugets.yaml](template-pack-and-sign-all-nugets.yaml)
  - [template-pack-and-sign-nuget.yaml](template-pack-and-sign-nuget.yaml) `('$(Build.SourcesDirectory)\src\Microsoft.Identity.Abstractions')`
  - 'Copy Files from `$(Build.SourcesDirectory)` to: `$(Build.ArtifactStagingDirectory)\packages'`
  - Sign Packages `'('$(Build.ArtifactStagingDirectory)\packages')`
- [template-publish-packages-and-symbols.yaml](template-publish-packages-and-symbols.yaml)
  - 'Verify packages are signed'
  - 'Publish Artifact: packages'
  - 'Publish packages to MyGet'
  - 'Publish packages to VSTS feed'
  - 'Publish symbols'
- [template-publish-analysis-and-cleanup.yaml](template-publish-analysis-and-cleanup.yaml)
  - Publish Security Analysis Logs'
  - 'TSA upload to Codebase: Microsoft Identity Abstractions .NET Stamp: Azure'
  - 'Clean Agent Directories'