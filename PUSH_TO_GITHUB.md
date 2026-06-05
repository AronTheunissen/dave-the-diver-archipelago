# Pushing to GitHub

## Quick Push (if repository exists)

```powershell
cd dave-the-diver-archipelago
git push -u origin main
```

## Step-by-Step Instructions

### 1. Create the Repository on GitHub

1. Go to https://github.com/AronTheunissen
2. Click the **"+"** button in top right, select **"New repository"**
3. Repository name: `dave-the-diver-archipelago`
4. Description: `Archipelago multiworld randomizer support for Dave the Diver`
5. **Keep it public** (or private if you prefer)
6. **DO NOT** initialize with README, .gitignore, or license (we already have these)
7. Click **"Create repository"**

### 2. Push Your Code

**Note:** Git has already been initialized and configured. Just run:

```powershell
cd dave-the-diver-archipelago
git push -u origin main
```

If you're prompted for credentials:
- **Username:** AronTheunissen
- **Password:** Use a Personal Access Token (not your GitHub password)

### 3. Create a Personal Access Token (if needed)

If you don't have a token:

1. Go to https://github.com/settings/tokens
2. Click **"Generate new token"** → **"Generate new token (classic)"**
3. Give it a name like "Dave the Diver Archipelago"
4. Select scopes: **`repo`** (full control of private repositories)
5. Click **"Generate token"**
6. **Copy the token immediately** (you won't see it again!)
7. Use this token as your password when pushing

### 4. Verify on GitHub

After pushing, go to:
https://github.com/AronTheunissen/dave-the-diver-archipelago

You should see all your files!

## Future Commits

After making changes:

```powershell
cd dave-the-diver-archipelago

# Stage changes
git add .

# Commit with a message
git commit -m "Add more items and locations"

# Push to GitHub
git push
```

## Useful Git Commands

```powershell
# Check status
git status

# See what changed
git diff

# View commit history
git log --oneline

# Undo unstaged changes
git checkout -- filename.py

# Create a new branch
git checkout -b feature/new-items

# Switch branches
git checkout main

# Pull latest changes
git pull
```

## .gitignore Already Configured

The following are already ignored:
- Python virtual environment (`apworld/venv/`)
- Python cache files (`__pycache__/`)
- Archipelago repository (`tools/Archipelago/`)
- Game reference DLLs (`client/lib/*.dll`)
- Personal config (`client/GamePath.props`)
- Build artifacts (`client/bin/`, `client/obj/`)

## Recommended: Add GitHub Repository Description

After creating the repository, add these topics:
- `archipelago`
- `randomizer`
- `dave-the-diver`
- `multiworld`
- `bepinex`
- `mod`

This helps people discover your project!

---

**Current Status:**
- ✅ Git initialized
- ✅ Initial commit created
- ✅ Remote configured: https://github.com/AronTheunissen/dave-the-diver-archipelago.git
- ⏳ Ready to push (create repository on GitHub first)
