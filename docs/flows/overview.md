# Overview

## System Context

The system works with three systems:
- Public (reporting)
- Report Status (event grouping)
- Risk Management (tools for professional partners).

This overview describes how two of these systems work together to manage flood reporting:

- **Public** — the public-facing tool where members of the public submit flood reports. Each submission is a *Flood Report Source*; it is the raw, unverified data provided by a reporter.
- **Report Status** — the back-office tool used to organise reports into *Flood Reports*. It receives flood report sources and creates and manages the authoritative *Flood Report* — the single version of the truth for a discrete flooding event. It maintains the links to the original information so a flood report may have multiple linked sources.

> **Naming note:** The term "Flood Report" means different things in each system. In the Public system we use the term with the end users, but it is better understood as a *Flood Report Source* — a submission that may or may not correspond to a real flooding event. In Report Status, *Flood Report* is the authoritative record.

---

## System Relationships

```mermaid
flowchart TD
subgraph Public["Public"]
    PR["Flood Report Source<br>(public submission)"]
    EC["Eligibility Check"]
    INV["Investigation<br>(0..1)"]
    PR -->|"has one (optional)"| INV
    PR -->|"has one"| EC
end

subgraph Contracts["Contracts"]
    MSG["FloodReportSourceCreated<br>(message)"]
end

subgraph ReportStatus["Report Status"]
    FR["Flood Report<br>(authoritative record)"]
    FRS["Flood Report Source<br>(filtered copy of public submission)"]
    FC["Flood Causes"]
    ORG["Organisations"]

    FRS -->|"linked to one or many"| FR
    FRS -->|"has many"| FC
    FRS -->|"has many"| ORG
end

PR -->|"publishes"| MSG
MSG -->|"consumed by"| FRS
```

## Flood Report Lifecycle (Report Status)
```mermaid
flowchart LR
    A["FloodReportSourceCreated<br>message received"] --> B["FloodReportSource created<br>(copy of public submission)"]
    B --> C{"Existing Flood Report<br>nearby?"}
    C -->|"Yes"| D["Link to existing<br>Flood Report"]
    C -->|"No"| E["Create new<br>Flood Report"]
    D --> F["Flood Report updated"]
    E --> F
    F --> G["Flood Report Created/Updated message<br>Sent to Service Bus"]
    G --> H["Professional review<br>& Investigation (optional)"]
```

---

## Key Entities — Report Status

| Entity | Cardinality | Description |
|---|---|---|
| `FloodReport` | — | The authoritative record of a flooding event |
| `FloodReportSource` | one or many per `FloodReport` | A submission that contributed to this flood report |
| `Investigation` | 0..1 per `FloodReportSource` | A follow on form completed by reporter to assist investigation |

