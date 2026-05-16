import client from './client'

export const usersApi = {
  getMe: () => client.get('/user/me'),
  getById: (id) => client.get(`/user/${id}`),
  getAll: () => client.get('/user'),
  update: (data) => client.put('/user/me', data),
}
