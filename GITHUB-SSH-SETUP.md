# GitHub SSH Setup Guide

Complete guide for setting up SSH authentication with GitHub and pushing code to your repository.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Understanding SSH Keys](#understanding-ssh-keys)
- [Step 1: Generate SSH Keys](#step-1-generate-ssh-keys)
- [Step 2: Add SSH Key to GitHub](#step-2-add-ssh-key-to-github)
- [Step 3: Configure Git Repository](#step-3-configure-git-repository)
- [Step 4: Push Code to GitHub](#step-4-push-code-to-github)
- [Troubleshooting](#troubleshooting)
- [Daily Git Workflow](#daily-git-workflow)

---

## Prerequisites

Before starting, ensure you have:
- ✅ Git installed (`git --version`)
- ✅ A GitHub account (https://github.com)
- ✅ Terminal/Command Line access
- ✅ Code ready to push

## Understanding SSH Keys

### What are SSH Keys?

SSH (Secure Shell) keys are a pair of cryptographic keys used for secure authentication:

- **Private Key** (`id_rsa`): Stays on your computer, never share this
- **Public Key** (`id_rsa.pub`): Shared with services like GitHub

### Why Use SSH Instead of HTTPS?

| Feature | SSH | HTTPS |
|---------|-----|-------|
| Authentication | Automatic with keys | Requires username/password each time |
| Security | Very secure | Secure but less convenient |
| Setup | One-time setup | Repeated authentication |
| Recommended For | Regular development | Quick/temporary access |

---

## Step 1: Generate SSH Keys

### 1.1 Check for Existing SSH Keys

```bash
# List existing SSH keys
ls -la ~/.ssh/

# Look for these files:
# - id_rsa (private key)
# - id_rsa.pub (public key)
# - id_ed25519 (newer format private key)
# - id_ed25519.pub (newer format public key)
```

### 1.2 Generate New SSH Keys (if needed)

If you don't have SSH keys, generate them:

#### Option A: RSA Key (Most Compatible)
```bash
# Generate RSA key with 4096 bits
ssh-keygen -t rsa -b 4096 -C "your_email@example.com"

# When prompted:
# 1. Press Enter to save to default location (~/.ssh/id_rsa)
# 2. Enter a passphrase (optional but recommended)
# 3. Confirm the passphrase
```

#### Option B: ED25519 Key (More Modern, Recommended)
```bash
# Generate ED25519 key (faster and more secure)
ssh-keygen -t ed25519 -C "your_email@example.com"

# When prompted:
# 1. Press Enter to save to default location (~/.ssh/id_ed25519)
# 2. Enter a passphrase (optional but recommended)
# 3. Confirm the passphrase
```

### 1.3 Start SSH Agent and Add Key

```bash
# Start the SSH agent in the background
eval "$(ssh-agent -s)"

# Add your SSH private key to the agent
ssh-add ~/.ssh/id_rsa
# Or for ED25519:
ssh-add ~/.ssh/id_ed25519
```

### 1.4 Display Your Public Key

```bash
# For RSA key
cat ~/.ssh/id_rsa.pub

# For ED25519 key
cat ~/.ssh/id_ed25519.pub

# Copy to clipboard (macOS)
pbcopy < ~/.ssh/id_rsa.pub

# Copy to clipboard (Linux with xclip)
xclip -selection clipboard < ~/.ssh/id_rsa.pub

# Copy to clipboard (Windows Git Bash)
clip < ~/.ssh/id_rsa.pub
```

**Example Output:**
```
ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQC... your_email@example.com
```

---

## Step 2: Add SSH Key to GitHub

### 2.1 Open GitHub SSH Settings

**Method 1: Direct Link**
```bash
# Open in browser (macOS)
open https://github.com/settings/ssh/new

# Linux
xdg-open https://github.com/settings/ssh/new

# Windows
start https://github.com/settings/ssh/new
```

**Method 2: Manual Navigation**
1. Go to https://github.com
2. Click your profile picture (top right)
3. Click **Settings**
4. Click **SSH and GPG keys** (left sidebar)
5. Click **New SSH key** button

### 2.2 Add Your SSH Key

1. **Title**: Enter a descriptive name
   - Example: `MacBook Pro - Development`
   - Example: `Home Desktop - Windows`
   - Example: `Work Laptop`

2. **Key Type**: Select **Authentication Key**

3. **Key**: Paste your public key
   - Should start with `ssh-rsa` or `ssh-ed25519`
   - Should end with your email

4. Click **Add SSH key**

5. **Confirm**: Enter your GitHub password if prompted

### 2.3 Verify SSH Connection

Test that your SSH key is working:

```bash
# Test SSH connection to GitHub
ssh -T git@github.com

# Expected output:
# Hi USERNAME! You've successfully authenticated, but GitHub does not provide shell access.
```

**Common First-Time Prompts:**
```bash
# You may see this the first time:
The authenticity of host 'github.com (IP)' can't be established.
ED25519 key fingerprint is SHA256:+DiY3wvvV6TuJJhbpZisF/zLDA0zPMSvHdkr4UvCOqU.
Are you sure you want to continue connecting (yes/no/[fingerprint])?

# Type: yes
```

---

## Step 3: Configure Git Repository

### 3.1 Configure Git User Information

Set your name and email (used in commits):

```bash
# Set global configuration (applies to all repositories)
git config --global user.name "Your Name"
git config --global user.email "your_email@example.com"

# Or set for current repository only
cd /path/to/your/repository
git config user.name "Your Name"
git config user.email "your_email@example.com"

# Verify configuration
git config --list
```

### 3.2 Initialize Git Repository (if not already done)

```bash
# Navigate to your project
cd /path/to/your/project

# Initialize git repository
git init

# Check status
git status
```

### 3.3 Create .gitignore File

Create a `.gitignore` file to exclude unnecessary files:

```bash
# For C#/.NET projects
cat > .gitignore << 'EOF'
# Build artifacts
bin/
obj/
*.user
*.suo
.vs/

# NuGet packages
packages/
*.nupkg

# Database files
*.db
*.sqlite

# Logs
*.log

# OS files
.DS_Store
Thumbs.db

# Sensitive files
appsettings.Development.json
*.pfx
EOF
```

### 3.4 Add Files to Git

```bash
# Add all files
git add .

# Or add specific files
git add README.md
git add SchoolApi/

# Check what will be committed
git status
```

### 3.5 Create Initial Commit

```bash
# Commit with a descriptive message
git commit -m "Initial commit: Complete School Management System

- Backend API with ASP.NET Core and MySQL
- Blazor WebAssembly frontend
- iOS MAUI mobile app
- Full Docker containerization
- Comprehensive documentation"

# Verify commit was created
git log --oneline
```

---

## Step 4: Push Code to GitHub

### 4.1 Create GitHub Repository

1. Go to https://github.com/new
2. **Repository name**: `school-app` (or your preferred name)
3. **Description**: "Complete school management system with C# and .NET"
4. **Visibility**: 
   - ✅ Public (anyone can see)
   - ✅ Private (only you and collaborators)
5. **Important**: 
   - ❌ Do NOT check "Add a README file"
   - ❌ Do NOT add .gitignore
   - ❌ Do NOT add license
6. Click **Create repository**

### 4.2 Add Remote Repository

After creating the repository, GitHub shows you commands. Use the SSH URL:

```bash
# Add remote with SSH URL (RECOMMENDED)
git remote add origin git@github.com:USERNAME/REPOSITORY.git

# Example:
git remote add origin git@github.com:attiacloudgalaxy/school-app.git

# Verify remote was added
git remote -v

# Output should show:
# origin  git@github.com:USERNAME/REPOSITORY.git (fetch)
# origin  git@github.com:USERNAME/REPOSITORY.git (push)
```

### 4.3 Push Code to GitHub

```bash
# Push to main branch and set upstream
git push -u origin main

# The -u flag sets the upstream branch
# Future pushes only need: git push
```

**Expected Output:**
```
Enumerating objects: 144, done.
Counting objects: 100% (144/144), done.
Delta compression using up to 10 threads
Compressing objects: 100% (129/129), done.
Writing objects: 100% (144/144), 364.13 KiB | 1.85 MiB/s, done.
Total 144 (delta 18), reused 0 (delta 0)
To github.com:USERNAME/REPOSITORY.git
 * [new branch]      main -> main
branch 'main' set up to track 'origin/main'.
```

### 4.4 Verify on GitHub

1. Open your repository: `https://github.com/USERNAME/REPOSITORY`
2. You should see:
   - ✅ All your files
   - ✅ README.md displayed
   - ✅ Commit history
   - ✅ File structure

---

## Troubleshooting

### Problem: "Permission denied (publickey)"

**Cause**: SSH key not added to GitHub or SSH agent

**Solution:**
```bash
# 1. Verify SSH key exists
ls -la ~/.ssh/

# 2. Add key to SSH agent
eval "$(ssh-agent -s)"
ssh-add ~/.ssh/id_rsa

# 3. Test connection
ssh -T git@github.com

# 4. If still fails, regenerate key and add to GitHub
```

### Problem: "remote origin already exists"

**Cause**: Remote already configured

**Solution:**
```bash
# Check current remote
git remote -v

# Remove existing remote
git remote remove origin

# Add correct remote
git remote add origin git@github.com:USERNAME/REPOSITORY.git
```

### Problem: "Could not read from remote repository"

**Cause**: Wrong remote URL or SSH key issue

**Solution:**
```bash
# Verify you're using SSH URL (not HTTPS)
git remote -v

# If using HTTPS, change to SSH
git remote set-url origin git@github.com:USERNAME/REPOSITORY.git

# Test SSH connection
ssh -T git@github.com
```

### Problem: "Authentication failed" with HTTPS URL

**Cause**: Using HTTPS URL instead of SSH

**Solution:**
```bash
# Change from HTTPS to SSH
git remote set-url origin git@github.com:USERNAME/REPOSITORY.git

# Verify change
git remote -v
```

### Problem: "src refspec main does not match any"

**Cause**: No commits in the repository

**Solution:**
```bash
# Create a commit first
git add .
git commit -m "Initial commit"
git push -u origin main
```

### Problem: SSH asks for passphrase every time

**Cause**: SSH key not added to keychain

**Solution (macOS):**
```bash
# Add to macOS keychain
ssh-add --apple-use-keychain ~/.ssh/id_rsa

# Configure SSH to use keychain
cat >> ~/.ssh/config << 'EOF'
Host github.com
  AddKeysToAgent yes
  UseKeychain yes
  IdentityFile ~/.ssh/id_rsa
EOF
```

**Solution (Linux):**
```bash
# Add key to agent permanently
eval "$(ssh-agent -s)"
ssh-add ~/.ssh/id_rsa

# Add to shell startup
echo 'eval "$(ssh-agent -s)"' >> ~/.bashrc
echo 'ssh-add ~/.ssh/id_rsa 2>/dev/null' >> ~/.bashrc
```

---

## Daily Git Workflow

### Making Changes and Pushing Updates

```bash
# 1. Check current status
git status

# 2. Pull latest changes (if working with others)
git pull

# 3. Make your code changes
# ... edit files ...

# 4. See what changed
git status
git diff

# 5. Stage changes
git add .
# Or specific files:
git add SchoolApi/Controllers/NewController.cs

# 6. Commit changes
git commit -m "Add new feature: Student grades"

# 7. Push to GitHub
git push

# That's it! Your changes are now on GitHub
```

### Viewing History

```bash
# View commit history
git log

# Compact view
git log --oneline

# Show changes in last commit
git show

# View specific commit
git show COMMIT_HASH
```

### Creating Branches

```bash
# Create and switch to new branch
git checkout -b feature/new-feature

# Make changes and commit
git add .
git commit -m "Add new feature"

# Push branch to GitHub
git push -u origin feature/new-feature

# Switch back to main
git checkout main

# Merge feature branch
git merge feature/new-feature

# Delete branch
git branch -d feature/new-feature
git push origin --delete feature/new-feature
```

### Undoing Changes

```bash
# Discard changes in working directory
git checkout -- filename

# Unstage files
git reset HEAD filename

# Undo last commit (keep changes)
git reset --soft HEAD~1

# Undo last commit (discard changes)
git reset --hard HEAD~1
```

---

## Best Practices

### ✅ Do's

1. **Commit Often**: Small, focused commits are better than large ones
2. **Write Clear Commit Messages**: Describe what and why
3. **Pull Before Push**: Always pull latest changes before pushing
4. **Use Branches**: Create branches for new features
5. **Review Changes**: Use `git diff` before committing
6. **Use .gitignore**: Never commit build artifacts or sensitive data
7. **Secure Your Keys**: Never share private SSH keys

### ❌ Don'ts

1. **Don't Commit Secrets**: No passwords, API keys, or tokens
2. **Don't Commit Large Files**: Use Git LFS for large files
3. **Don't Force Push**: Avoid `git push --force` on shared branches
4. **Don't Commit Everything**: Use .gitignore appropriately
5. **Don't Share Private Keys**: Only share public keys

---

## Quick Reference Commands

### Setup
```bash
# Generate SSH key
ssh-keygen -t ed25519 -C "email@example.com"

# Start SSH agent and add key
eval "$(ssh-agent -s)"
ssh-add ~/.ssh/id_ed25519

# Test GitHub connection
ssh -T git@github.com

# Configure git
git config --global user.name "Your Name"
git config --global user.email "email@example.com"
```

### Repository Operations
```bash
# Initialize repository
git init

# Add remote
git remote add origin git@github.com:USER/REPO.git

# Check status
git status

# Stage all changes
git add .

# Commit
git commit -m "Message"

# Push
git push -u origin main
```

### Daily Workflow
```bash
# Pull latest
git pull

# See changes
git status
git diff

# Stage, commit, push
git add .
git commit -m "Description"
git push
```

---

## Security Tips

### Protecting Your SSH Keys

1. **Set Passphrase**: Always use a strong passphrase
2. **File Permissions**: 
   ```bash
   chmod 700 ~/.ssh
   chmod 600 ~/.ssh/id_rsa
   chmod 644 ~/.ssh/id_rsa.pub
   ```
3. **Backup Keys**: Keep encrypted backup of private keys
4. **Rotate Keys**: Change SSH keys periodically
5. **Multiple Keys**: Use different keys for different services

### Protecting Sensitive Data

```bash
# Never commit these files:
# - Database files (*.db, *.sqlite)
# - Configuration with secrets (appsettings.Development.json)
# - API keys and tokens
# - SSL certificates (*.pfx, *.p12)
# - Build artifacts (bin/, obj/)

# Use environment variables for secrets
# Use .gitignore to exclude sensitive files
# Use git-secrets or similar tools to prevent accidental commits
```

---

## Additional Resources

### Official Documentation
- [GitHub SSH Documentation](https://docs.github.com/en/authentication/connecting-to-github-with-ssh)
- [Git Documentation](https://git-scm.com/doc)
- [GitHub Guides](https://guides.github.com/)

### Learning Git
- [GitHub Learning Lab](https://lab.github.com/)
- [Git Interactive Tutorial](https://learngitbranching.js.org/)
- [Pro Git Book (Free)](https://git-scm.com/book/en/v2)

### Tools
- [GitHub Desktop](https://desktop.github.com/) - GUI for Git
- [GitKraken](https://www.gitkraken.com/) - Visual Git client
- [SourceTree](https://www.sourcetreeapp.com/) - Free Git GUI

---

## Summary

You've learned how to:
- ✅ Generate SSH keys for secure authentication
- ✅ Add SSH keys to GitHub
- ✅ Configure Git repository
- ✅ Push code to GitHub using SSH
- ✅ Troubleshoot common issues
- ✅ Follow daily Git workflow
- ✅ Maintain security best practices

**Your code is now securely backed up on GitHub!** 🎉

For questions or issues, refer to the [Troubleshooting](#troubleshooting) section or GitHub's official documentation.

---

**Last Updated**: October 29, 2025  
**Repository**: https://github.com/attiacloudgalaxy/school-app  
**Author**: Dr. Attia (mohammed.attia@live.co.uk)
