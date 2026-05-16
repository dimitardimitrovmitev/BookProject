# BookProject Frontend

Vue 3 + Vite frontend for BookProject API.

## Setup

```bash
npm install
npm run dev
```

Runs at http://localhost:5173

## API Base URL

Edit `src/api/client.js` → `baseURL` to match your backend port.
Default assumes `http://localhost:5175`.

## Project Structure

```
src/
├── api/           # Axios API services (auth, books, characters, reviews, scenes)
├── assets/        # main.css — full design system
├── components/
│   ├── common/    # Sidebar, Pagination, StarRating, StarInput, Modals
│   ├── books/     # BookCard, BookFormModal
│   ├── characters/# CharacterCard, CharacterFormModal
│   ├── reviews/   # BookReviewModal, CharacterReviewModal
│   └── scenes/
├── composables/   # useToast
├── router/        # Vue Router with auth guards
├── stores/        # Pinia auth store (JWT decode)
└── views/
    ├── LoginView.vue
    ├── RegisterView.vue
    ├── BooksView.vue          ← browse + admin CRUD
    ├── BookDetailView.vue     ← covers, characters, reviews, library mgmt
    ├── CharactersView.vue     ← browse + admin CRUD
    ├── CharacterDetailView.vue← 5 sub-ratings, character reviews
    ├── MyLibraryView.vue      ← reading list, status tabs
    ├── ScenesView.vue         ← scene management
    └── ProfileView.vue        ← stats + account settings
```

## Features
- JWT auth with auto-redirect guards
- Admin-only controls (add/edit/delete books & characters) shown based on JWT role
- Book covers via OpenLibrary CDN
- 5-dimensional character review ratings with computed overall score
- Reading status management (WantToRead / Reading / Read / DNF)
- Personal library with private ratings
- Dark literary design system (Playfair Display + Crimson Pro)
