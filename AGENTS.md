# Repository Guidelines

## Project Structure & Module Organization
The solution `matias.net.sln` groups three projects. `common-matias.net/` provides shared logic and generated gRPC stubs in `matias_service/`. `matias-console.net/` hosts the CLI entry point that reads Seppo configuration from `config/`. Installer assets live in `MatiasInstaller/`, while top-level `protos/` contains the `.proto` files used to regenerate gRPC classes. Build artefacts land under each project's `bin/` and `obj/` folders created on demand.

## Build, Test, and Development Commands
Run `nuget restore matias.net.sln` once to install packages listed in each `packages.config`. Build everything via `msbuild matias.net.sln /p:Configuration=Debug` (use `Release` for installer builds). During development it is convenient to build a single project (`msbuild matias-console.net/matias-console.net.csproj`). After a build, run the CLI with `bin/Debug/matias-console.net.exe` from the repo root so it can resolve the local `config` file.

## Coding Style & Naming Conventions
This codebase targets the .NET Framework; use 4-space indentation, braces on new lines, and `var` only when the type is obvious. Follow PascalCase for public types, methods, and properties (`SeppoClient`, `CreateClient`), camelCase for fields and locals (`seppoIp`). Keep namespaces aligned with folder names (`common_matias`). Regenerate gRPC clients by re-running the proto tooling before committing manually edited files.

## Testing Guidelines
There is no automated test project yet. When adding tests, create a sibling project such as `common-matias.net.Tests` using NUnit or xUnit so it can run on .NET Framework, and load it into the solution. Adopt fixture names mirroring the class under test and method names describing the scenario in Finnish or English (`SeppoClientTests.ConnectsWithConfiguredPort`). Execute tests with `dotnet test` once the project targets a supported SDK; until then rely on `mstest` or ReSharper test runners in Visual Studio.

## Commit & Pull Request Guidelines
Existing commits use concise Finnish summaries (e.g., `Päivitetty proto.`); keep messages short, start with a capital letter, and highlight the primary change. Include additional context in the body when touching both proto files and generated sources. Pull requests should describe the feature, list manual verification steps (CLI run, installer build), and mention affected configuration or proto files. Attach logs or screenshots when tweaking installer UI or console output to ease review.

## Proto & Configuration Tips
Update `.proto` definitions first, regenerate stubs, then recompile to ensure `MatiasServiceGrpc.cs` stays in sync. Check in any changes to the sample `config/` file only when values are sanitized; otherwise document required keys in the PR.
