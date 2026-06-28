# Equipo de Agentes — Enterprise Engineering

Este repositorio incluye un equipo de agentes especializados para operar como un departamento de tecnología empresarial completo.

## Agente Orquestador

| Agente | Skill | Cuándo usar |
|--------|-------|-------------|
| **Enterprise Engineering Agent** | `enterprise-engineering-agent` | Análisis multidisciplinario, soluciones production-ready, evaluación completa de requerimientos |

Invócalo mencionando el skill o pidiendo el "agente enterprise de ingeniería".

## Agentes Especializados

| # | Rol | Skill | Activar cuando... |
|---|-----|-------|-------------------|
| 1 | Arquitecto de Software Senior | `software-architect-agent` | Diseño, arquitectura, DDD, CQRS, microservicios |
| 2 | Desarrollador Senior Full Stack | `senior-fullstack-developer-agent` | Implementación de código, APIs, frontend, bugs |
| 3 | Ingeniero DevOps Senior | `devops-engineer-agent` | CI/CD, Docker, K8s, pipelines, despliegue |
| 4 | Especialista en Seguridad | `security-specialist-agent` | Vulnerabilidades, OWASP, Auth/AuthZ, secrets |
| 5 | Analista de Calidad de Código | `code-quality-analyst-agent` | Refactorización, code smells, deuda técnica |
| 6 | Ingeniero de Automatización QA | `qa-automation-engineer-agent` | Tests unitarios, integración, E2E, test plans |
| 7 | Technical PM | `technical-pm-agent` | Backlog, sprints, user stories, riesgos, roadmap |

## Motor de decisión

```
Solicitud → Tipo detectado → Agente(s) activado(s) → Respuesta consolidada
```

- **Diseño** → Arquitecto
- **Código** → Desarrollador
- **Infraestructura** → DevOps
- **Vulnerabilidades** → Seguridad
- **Refactorización** → Calidad
- **Testing** → QA Automation
- **Gestión** → Technical PM

Solicitudes que cruzan múltiples áreas activan varios agentes en una respuesta unificada.

## Formato de respuesta

El orquestador sigue 10 secciones estándar:

1. Resumen Ejecutivo
2. Análisis Técnico
3. Evaluación por Especialidad
4. Riesgos Detectados
5. Recomendación Principal
6. Plan de Implementación
7. Código / YAML / Scripts
8. Pruebas Recomendadas
9. Criterios de Aceptación
10. Mejoras Futuras

Ver plantilla completa en `.cursor/skills/enterprise-engineering-agent/response-template.md`.

## Estructura de archivos

```
.cursor/skills/
├── enterprise-engineering-agent/   # Orquestador principal
│   ├── SKILL.md
│   ├── response-template.md
│   └── specializations.md
├── software-architect-agent/
├── senior-fullstack-developer-agent/
├── devops-engineer-agent/
├── security-specialist-agent/
├── code-quality-analyst-agent/
├── qa-automation-engineer-agent/
└── technical-pm-agent/
```

## Ejemplos de uso

```
@enterprise-engineering-agent Diseña la arquitectura para un módulo de préstamos de libros
```

```
@software-architect-agent Evalúa si debemos migrar a microservicios
```

```
@security-specialist-agent Audita el endpoint de creación de libros
```

```
@qa-automation-engineer-agent Crea tests de integración para el flujo CRUD de autores
```

```
@technical-pm-agent Descompón el epic "Sistema de reservas" en user stories
```

## Integración con el proyecto

Los agentes respetan la arquitectura de BookAPI:
- Clean Architecture con capas Domain / Application / Infrastructure / Api
- CQRS con MediatR
- EF Core + FluentValidation
- Tests en `UnitTests/` e `IntegrationTests/`
- Seguridad con skill `snyk-rules` tras cambios de código
