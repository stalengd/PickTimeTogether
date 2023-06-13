<!-- <svelte:options tag="svelte-app" /> -->

<script lang="ts">
    import { Router, Route, link } from 'svelte-routing';
    import { routes } from './routes';

    let pathname = location.pathname;
</script>

<Router>
    <nav class="navbar navbar-expand-lg navbar-dark">
        <div class="container">
            <a class="navbar-brand" href="/">
                <i class="svg-logo svg-lg mr-1"></i>
                Pick Time Together
            </a>
            <ul class="navbar-nav">
            {#each routes as {path, label, exact} (label) }
                <li class="nav-item">
                    <a href="{path}" class="nav-link" class:active={exact ? pathname == path : pathname.startsWith(path)} use:link>{label}</a>
                </li>
            {/each}
            </ul>
        </div>
    </nav>
    <div class="container">
        <div class="row p-4">
            <div id="content">
                {#each routes as {path, component}}
                    <Route path="{path}" component="{component}" />
                {/each}
            </div>
        </div>
    </div>
</Router>

<style>
    h1 {
        font-size: 5em;
    }
</style>