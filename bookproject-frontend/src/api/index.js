import client from './client'

export const charactersApi = {
  getAll: (params) => client.get('/character', { params }),
  getById: (id) => client.get(`/character/${id}`),
  getByBook: (bookId, params) => client.get(`/character/book/${bookId}`, { params }),
  create: (data) => client.post('/character/manual', data),
  update: (id, data) => client.put(`/character/${id}`, data),
  delete: (id) => client.delete(`/character/${id}`),
}

export const bookReviewsApi = {
  getAll: (params) => client.get('/bookreview', { params }),
  getMy: (params) => client.get('/bookreview/my', { params }),
  getById: (id) => client.get(`/bookreview/${id}`),
  getByBook: (bookId) => client.get(`/bookreview/book/${bookId}`),
  create: (data) => client.post('/bookreview', data),
  update: (id, data) => client.put(`/bookreview/${id}`, data),
  delete: (id) => client.delete(`/bookreview/${id}`),
}

export const characterReviewsApi = {
  getAll: (params) => client.get('/characterreview', { params }),
  getMy: (params) => client.get('/characterreview/my', { params }),
  getById: (id) => client.get(`/characterreview/${id}`),
  create: (data) => client.post('/characterreview', data),
  update: (id, data) => client.put(`/characterreview/${id}`, data),
  delete: (id) => client.delete(`/characterreview/${id}`),
}

export const userBooksApi = {
  getMy: (params) => client.get('/userbook/my', { params }),
  getByUser: (userId, params) => client.get(`/userbook/user/${userId}`, { params }),
  add: (data) => client.post('/userbook', data),
  updateStatus: (bookId, data) => client.put(`/userbook/book/${bookId}/status`, data),
  rate: (bookId, data) => client.put(`/userbook/book/${bookId}/rate`, data),
  remove: (bookId) => client.delete(`/userbook/book/${bookId}`),
}

export const scenesApi = {
  getMy: () => client.get('/scene/my'),
  getById: (id) => client.get(`/scene/${id}`),
  create: (data) => client.post('/scene', data),
  update: (id, data) => client.put(`/scene/${id}`, data),
  delete: (id) => client.delete(`/scene/${id}`),
}
