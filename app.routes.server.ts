import { RenderMode, ServerRoute } from '@angular/ssr';

// Everything in this pharmacy app is per-user, JWT-protected, and depends on
// a live backend. Prerendering would force the SSR server to call the API at
// build time and stalls the page in "buffering" state when the backend is
// unreachable. Use Client rendering for every route — the SSR layer just
// serves the Angular shell and lets the browser hydrate it.
export const serverRoutes: ServerRoute[] = [
  {
    path: '**',
    renderMode: RenderMode.Client
  }
];
