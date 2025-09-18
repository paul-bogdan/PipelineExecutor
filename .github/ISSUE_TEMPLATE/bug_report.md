name: "🐞 Bug Report"
description: Report a bug or unexpected behavior
title: "[Bug]: "
labels: ["bug"]
body:
  - type: markdown
    attributes:
      value: |
        Thanks for taking the time to fill out a bug report! Please provide as much detail as possible.
  - type: input
    id: version
    attributes:
      label: Version
      description: Which version of PipelineExecutor are you using?
      placeholder: "e.g., v1.0.0 or main branch"
    validations:
      required: true
  - type: textarea
    id: description
    attributes:
      label: Describe the bug
      description: What happened? What did you expect to happen?
      placeholder: "Clear and concise description of the issue."
    validations:
      required: true
  - type: textarea
    id: reproduction
    attributes:
      label: Steps to reproduce
      description: Provide steps to reproduce the issue.
      placeholder: "1. Run ...\n2. Execute ...\n3. See error"
    validations:
      required: true
  - type: textarea
    id: logs
    attributes:
      label: Relevant log output
      description: Paste any error messages or stack traces.
      render: shell
  - type: input
    id: environment
    attributes:
      label: Environment
      description: OS, .NET version, IDE, etc.
      placeholder: "Windows 11, .NET 6, Visual Studio 2022"
