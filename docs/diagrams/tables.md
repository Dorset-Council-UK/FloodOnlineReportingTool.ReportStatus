# Table and Relationship Diagrams

## Simple relationship diagrams
```mermaid
erDiagram
    FloodLocation ||--o{ FloodLocationFloodReport : "has many"
    FloodProperty ||--o{ FloodPropertyFloodReport : "has many"
    FloodRoad ||--o{ FloodRoadFloodReport : "has many"
    FloodReport ||--o{ FloodReportSource : "has many"
    FloodLocationFloodReport ||--|| FloodReport : "has one"
    FloodPropertyFloodReport ||--|| FloodReport : "has one"
    FloodRoadFloodReport ||--|| FloodReport : "has one"
```
