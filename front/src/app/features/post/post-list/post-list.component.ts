import { Component } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { Post } from '../modules/post-list-grid';
import { PostListRequest } from '../modules/post-list-request';
import { FormsModule } from '@angular/forms';
import { PostService } from '../services/post.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-post-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, FormsModule],
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.css'
})


export class PostListComponent {
  displayedColumns: string[] = ['id', 'author', 'authorId', 'likes', 'popularity', 'reads', 'tags'];
  dataSource: Post[] = [];
  /*
    The validation should be a service to validate all inputs
    that have to be validated in the form and return an array with the results.
    For this test purpose, I'm using a single variable.
  */
  isTagValid: boolean = true;
  filters: PostListRequest;

  constructor(private postService: PostService)
  {
    this.filters = {
      tags: '',
      sortBy: '',
      direction: ''
    }
  }

  onSearch() {
    if(this.filters.tags === "")
    {
      this.isTagValid = false;
      this.dataSource = [];
    }
    else
    {
      this.isTagValid = true;
      this.postService.getPosts(this.filters)
                      .subscribe({
                        next: (response) => {
                          this.dataSource = response.posts;
                        },
                        error: (response) => {
                          //here we can implement a toast manager to show a more user friendly message. =)
                          alert(response.error);
                        }
                      });
    }
  }
}




