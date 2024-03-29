import { Injectable } from '@angular/core';
import { PostListRequest } from '../modules/post-list-request';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { PostListResponse } from '../modules/post-list-response';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private http: HttpClient) { }

  getPosts(filters: PostListRequest):  Observable<PostListResponse> {
    return this.http.get<PostListResponse>('https://localhost:7171/posts', {params: {...filters}});
  }
}
