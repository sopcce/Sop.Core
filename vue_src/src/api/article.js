import request from '@/utils/request'

export function fetchList(query) {
  return request({
    url: '/api/Article/GetList',
    method: 'get',
    params: query
  })
}

export function fetchArticle(id) {
  return request({
    url: '/api/Article/detail',
    method: 'get',
    params: { id }
  })
}

export function fetchPv(pv) {
  return request({
    url: '/api/Article/pv',
    method: 'get',
    params: { pv }
  })
}

export function createArticle(data) {
  return request({
    url: '/api/Article/create',
    method: 'post',
    data
  })
}

export function updateArticle(data) {
  return request({
    url: '/api/Article/update',
    method: 'post',
    data
  })
}
