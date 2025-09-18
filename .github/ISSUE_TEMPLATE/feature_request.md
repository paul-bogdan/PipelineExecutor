name: "🚀 Feature Request"
description: Suggest a new idea or improvement
title: "[Feature]: "
labels: ["enhancement"]
body:
  - type: markdown
    attributes:
      value: |
        Thanks for suggesting an improvement! Please describe your idea clearly.
  - type: input
    id: motivation
    attributes:
      label: Motivation
      description: Why do you need this feature?
      placeholder: "What problem does it solve?"
    validations:
      required: true
  - type: textarea
    id: solution
    attributes:
      label: Proposed solution
      description: How would you like this feature to work?
      placeholder: "Describe your idea"
    validations:
      required: true
  - type: textarea
    id: alternatives
    attributes:
      label: Alternatives considered
      description: Have you thought about different approaches?
  - type: textarea
    id: additional
    attributes:
      label: Additional context
      description: Add screenshots, references, or other context here.
