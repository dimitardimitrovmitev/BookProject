import client from './client'

export const booksApi = {
  getAll: (params) => client.get('/book', { params }),
  getById: (id) => client.get(`/book/${id}`),
  create: (data) => client.post('/book/manual', data),
  update: (id, data) => client.put(`/book/${id}`, data),
  delete: (id) => client.delete(`/book/${id}`),
}
