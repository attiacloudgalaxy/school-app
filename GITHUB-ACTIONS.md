# GitHub Actions CI/CD Documentation

Complete guide for the automated CI/CD pipeline using GitHub Actions for the School Management System.

## Table of Contents
- [Overview](#overview)
- [Workflows](#workflows)
- [Setup Instructions](#setup-instructions)
- [Secrets Configuration](#secrets-configuration)
- [Workflow Details](#workflow-details)
- [Badges](#badges)
- [Troubleshooting](#troubleshooting)
- [Best Practices](#best-practices)

---

## Overview

This project uses GitHub Actions for automated:
- ✅ Building and testing .NET applications
- ✅ Building and pushing Docker images
- ✅ Running integration tests
- ✅ Code quality analysis
- ✅ Security scanning
- ✅ Dependency updates

### Workflows Summary

| Workflow | Trigger | Purpose |
|----------|---------|---------|
| `ci-cd.yml` | Push to main/develop, PRs | Complete CI/CD pipeline |
| `docker-publish.yml` | Push to main, tags, releases | Build and publish Docker images |
| `dependency-check.yml` | Weekly schedule, manual | Check for outdated dependencies |

---

## Workflows

### 1. CI/CD Pipeline (`ci-cd.yml`)

**Triggers:**
- Push to `main` or `develop` branches
- Pull requests to `main`
- Manual dispatch

**Jobs:**

#### Job 1: Build Backend API
- Restores NuGet packages
- Builds SchoolApi project
- Runs tests (if available)
- Publishes artifacts
- Uploads build artifacts for 7 days

#### Job 2: Build Blazor Frontend
- Restores NuGet packages
- Builds SchoolApp project
- Publishes artifacts
- Uploads build artifacts for 7 days

#### Job 3: Build Docker Images
- Sets up Docker Buildx
- Logs in to Docker Hub (if credentials configured)
- Builds multi-platform images (amd64, arm64)
- Pushes images to Docker Hub
- Uses GitHub Actions cache for faster builds

#### Job 4: Code Quality Analysis
- Runs `dotnet format` to verify code formatting
- Checks for code style violations

#### Job 5: Security Scan
- Runs Trivy vulnerability scanner
- Scans filesystem for security issues
- Uploads results to GitHub Security tab

#### Job 6: Integration Tests
- Starts services with Docker Compose
- Waits for health checks
- Tests API endpoints
- Cleans up containers

#### Job 7: Deployment Status
- Aggregates results from all jobs
- Reports overall pipeline status

### 2. Docker Publish (`docker-publish.yml`)

**Triggers:**
- Push to `main` branch
- Creation of version tags (v*.*.*)
- Published releases
- Manual dispatch

**Features:**
- Multi-platform builds (amd64, arm64)
- Automatic versioning from tags
- Docker Hub metadata updates
- README sync to Docker Hub
- Optimized caching

**Image Tags Generated:**
- `latest` (main branch only)
- `main` (branch name)
- `v1.0.0` (semver tags)
- `v1.0` (major.minor)
- `v1` (major only)
- `main-abc1234` (branch + SHA)

### 3. Dependency Check (`dependency-check.yml`)

**Triggers:**
- Weekly (Mondays at 9:00 AM UTC)
- Manual dispatch

**Purpose:**
- Checks all .NET projects for outdated packages
- Generates summary report
- Helps maintain up-to-date dependencies

---

## Setup Instructions

### Step 1: Enable GitHub Actions

1. Go to your repository on GitHub
2. Click **Settings** → **Actions** → **General**
3. Under **Actions permissions**, select:
   - ✅ Allow all actions and reusable workflows
4. Under **Workflow permissions**, select:
   - ✅ Read and write permissions
   - ✅ Allow GitHub Actions to create and approve pull requests
5. Click **Save**

### Step 2: Configure Repository Secrets

Go to **Settings** → **Secrets and variables** → **Actions** → **New repository secret**

#### Required Secrets for Docker Publishing:

| Secret Name | Description | How to Get |
|-------------|-------------|------------|
| `DOCKER_USERNAME` | Docker Hub username | Your Docker Hub account name |
| `DOCKER_PASSWORD` | Docker Hub access token | Generate at https://hub.docker.com/settings/security |

**Creating Docker Hub Access Token:**
1. Log in to https://hub.docker.com
2. Go to **Account Settings** → **Security** → **New Access Token**
3. Name: "GitHub Actions"
4. Permissions: Read, Write, Delete
5. Click **Generate**
6. Copy the token (you can't see it again!)
7. Add as `DOCKER_PASSWORD` secret in GitHub

### Step 3: Push Workflows to GitHub

The workflows are already created in `.github/workflows/`. Push them:

```bash
git add .github/workflows/
git commit -m "Add GitHub Actions CI/CD workflows"
git push
```

### Step 4: Verify Workflows

1. Go to your repository on GitHub
2. Click **Actions** tab
3. You should see the workflows listed
4. The CI/CD pipeline will run automatically on the next push

---

## Secrets Configuration

### Docker Hub Setup

#### 1. Create Docker Hub Account (if you don't have one)
```bash
# Go to https://hub.docker.com/signup
```

#### 2. Create Access Token
1. Log in to Docker Hub
2. Click your username → **Account Settings**
3. Click **Security** → **New Access Token**
4. Description: "GitHub Actions - School App"
5. Access permissions: **Read, Write, Delete**
6. Click **Generate Token**
7. **Copy the token immediately** (you can't see it again)

#### 3. Add Secrets to GitHub
```bash
# Go to your repository settings
Settings → Secrets and variables → Actions → New repository secret

Secret 1:
Name: DOCKER_USERNAME
Value: your-dockerhub-username

Secret 2:
Name: DOCKER_PASSWORD
Value: paste-the-access-token-here
```

#### 4. Create Docker Hub Repositories

Create two repositories on Docker Hub:
- `your-username/school-api` (Backend API)
- `your-username/school-app` (Frontend)

Or they will be created automatically on first push.

### Optional Secrets

| Secret | Purpose | Required? |
|--------|---------|-----------|
| `DOCKER_USERNAME` | Docker Hub username | Yes (for Docker publishing) |
| `DOCKER_PASSWORD` | Docker Hub token | Yes (for Docker publishing) |
| `GITHUB_TOKEN` | GitHub API access | Auto-provided by GitHub |

---

## Workflow Details

### Running Workflows Manually

1. Go to **Actions** tab
2. Select the workflow (e.g., "CI/CD Pipeline")
3. Click **Run workflow** button
4. Select branch
5. Click **Run workflow**

### Viewing Workflow Results

1. **Actions** tab shows all workflow runs
2. Click on a workflow run to see details
3. Click on individual jobs to see logs
4. Download artifacts from successful builds

### Understanding Workflow Status

| Icon | Status | Meaning |
|------|--------|---------|
| ✅ | Success | All jobs passed |
| ❌ | Failure | One or more jobs failed |
| 🟡 | In Progress | Workflow is running |
| ⚪ | Queued | Waiting to start |
| ⊘ | Cancelled | Manually stopped |

---

## Badges

Add these badges to your README.md to show build status:

### CI/CD Status Badge
```markdown
![CI/CD](https://github.com/attiacloudgalaxy/school-app/workflows/CI/CD%20Pipeline/badge.svg)
```

### Docker Build Badge
```markdown
![Docker Build](https://github.com/attiacloudgalaxy/school-app/workflows/Docker%20Build%20and%20Push/badge.svg)
```

### Combined Badges
```markdown
[![CI/CD](https://github.com/attiacloudgalaxy/school-app/workflows/CI/CD%20Pipeline/badge.svg)](https://github.com/attiacloudgalaxy/school-app/actions)
[![Docker Build](https://github.com/attiacloudgalaxy/school-app/workflows/Docker%20Build%20and%20Push/badge.svg)](https://github.com/attiacloudgalaxy/school-app/actions)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
```

---

## Troubleshooting

### Problem: "No workflows found"

**Cause**: Workflows not in correct directory or syntax errors

**Solution:**
```bash
# Verify directory structure
ls -la .github/workflows/

# Should show:
# ci-cd.yml
# docker-publish.yml
# dependency-check.yml

# Check YAML syntax
yamllint .github/workflows/*.yml
```

### Problem: "Docker login failed"

**Cause**: Invalid Docker Hub credentials

**Solution:**
```bash
# 1. Verify secrets are set correctly
# Settings → Secrets and variables → Actions

# 2. Regenerate Docker Hub access token
# https://hub.docker.com/settings/security

# 3. Update DOCKER_PASSWORD secret
```

### Problem: "Permission denied"

**Cause**: Insufficient workflow permissions

**Solution:**
```bash
# Go to: Settings → Actions → General → Workflow permissions
# Select: "Read and write permissions"
# Check: "Allow GitHub Actions to create and approve pull requests"
```

### Problem: "Build failed - Unable to restore packages"

**Cause**: Network issue or package source unavailable

**Solution:**
```yaml
# Add retry logic to workflow:
- name: Restore dependencies
  run: dotnet restore SchoolApi/SchoolApi.csproj
  timeout-minutes: 10
  continue-on-error: false
```

### Problem: "Integration tests timeout"

**Cause**: Services taking too long to start

**Solution:**
```yaml
# Increase wait time in workflow:
- name: Wait for services
  run: |
    timeout 120 bash -c 'until docker-compose ps | grep healthy; do sleep 5; done'
```

### Problem: "Docker push failed - unauthorized"

**Cause**: Docker Hub token expired or invalid

**Solution:**
1. Generate new access token on Docker Hub
2. Update `DOCKER_PASSWORD` secret
3. Re-run workflow

---

## Best Practices

### 1. Branch Protection

Enable branch protection for `main`:

```bash
# Settings → Branches → Add rule

Branch name pattern: main

☑ Require a pull request before merging
☑ Require status checks to pass before merging
  - Select: build-api, build-frontend, integration-test
☑ Require branches to be up to date before merging
☑ Do not allow bypassing the above settings
```

### 2. Semantic Versioning

Use tags for releases:

```bash
# Create a new release
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0

# This triggers Docker images with version tags
```

### 3. Pull Request Workflow

```bash
# 1. Create feature branch
git checkout -b feature/new-feature

# 2. Make changes and commit
git add .
git commit -m "Add new feature"

# 3. Push to GitHub
git push origin feature/new-feature

# 4. Create Pull Request on GitHub
# 5. Wait for CI checks to pass
# 6. Merge after approval
```

### 4. Workflow Optimization

**Cache Dependencies:**
```yaml
- name: Cache NuGet packages
  uses: actions/cache@v3
  with:
    path: ~/.nuget/packages
    key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
```

**Parallel Jobs:**
- Independent jobs run in parallel automatically
- Speeds up pipeline execution

**Job Conditions:**
```yaml
# Only run on main branch
if: github.ref == 'refs/heads/main'

# Only on pull requests
if: github.event_name == 'pull_request'

# Skip CI on specific commits
if: "!contains(github.event.head_commit.message, '[skip ci]')"
```

### 5. Security Best Practices

✅ **Do:**
- Use secrets for sensitive data
- Rotate access tokens regularly
- Use least privilege for tokens
- Enable Dependabot for security updates
- Review workflow permissions

❌ **Don't:**
- Hardcode passwords or tokens
- Use write permissions unless needed
- Allow workflows from forks to access secrets
- Disable security scans

---

## Advanced Configuration

### Matrix Builds

Test on multiple .NET versions:

```yaml
strategy:
  matrix:
    dotnet-version: ['6.0.x', '7.0.x', '8.0.x']
    os: [ubuntu-latest, windows-latest, macos-latest]
    
steps:
- name: Setup .NET ${{ matrix.dotnet-version }}
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: ${{ matrix.dotnet-version }}
```

### Conditional Steps

```yaml
- name: Deploy to production
  if: github.ref == 'refs/heads/main' && github.event_name == 'push'
  run: |
    echo "Deploying to production..."
```

### Reusable Workflows

Create `.github/workflows/reusable-build.yml`:

```yaml
name: Reusable Build

on:
  workflow_call:
    inputs:
      project-path:
        required: true
        type: string

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Build
        run: dotnet build ${{ inputs.project-path }}
```

Use it:

```yaml
jobs:
  build-api:
    uses: ./.github/workflows/reusable-build.yml
    with:
      project-path: SchoolApi/SchoolApi.csproj
```

---

## Monitoring and Notifications

### GitHub Notifications

Configure in **Settings** → **Notifications**:
- Email notifications for failed workflows
- Slack/Discord integration via webhooks

### Status Checks

Required status checks ensure quality:
- All builds must pass
- Integration tests must succeed
- Security scans must complete

### Workflow Insights

View metrics in **Insights** → **Actions**:
- Workflow run times
- Success/failure rates
- Most active workflows
- Average duration

---

## Resources

### Official Documentation
- [GitHub Actions Docs](https://docs.github.com/en/actions)
- [Workflow Syntax](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)
- [Docker Build Push Action](https://github.com/docker/build-push-action)

### Useful Actions
- [actions/checkout](https://github.com/actions/checkout)
- [actions/setup-dotnet](https://github.com/actions/setup-dotnet)
- [docker/build-push-action](https://github.com/docker/build-push-action)
- [docker/login-action](https://github.com/docker/login-action)

### Tools
- [act](https://github.com/nektos/act) - Run GitHub Actions locally
- [actionlint](https://github.com/rhysd/actionlint) - Lint workflow files

---

## Summary

✅ **What You Get:**
- Automated building and testing
- Docker image publishing
- Code quality checks
- Security scanning
- Integration testing
- Dependency monitoring

🚀 **Next Steps:**
1. Configure Docker Hub secrets
2. Enable workflows by pushing to GitHub
3. Monitor first workflow run
4. Add status badges to README
5. Configure branch protection

**Your CI/CD pipeline is ready!** Every push triggers automated builds, tests, and deployments. 🎉

---

**Last Updated**: October 29, 2025  
**Repository**: https://github.com/attiacloudgalaxy/school-app  
**Author**: Dr. Attia (mohammed.attia@live.co.uk)
